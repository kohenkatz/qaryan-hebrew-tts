//    This file is part of Qaryan.
//
//    Qaryan is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Qaryan is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Qaryan.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Threading;
using System.Windows.Forms;
using Microsoft.DirectX.DirectSound;
using DirectSound=Microsoft.DirectX.DirectSound;
using System.Runtime.InteropServices;

namespace Qaryan.Audio
{
    delegate void PullAudio(byte[] buffer, int length);

    class SoundPlayer : IDisposable
    {
        private Device soundDevice;
        private SecondaryBuffer soundBuffer;
        private int samplesPerUpdate;
        private AutoResetEvent[] fillEvent = new AutoResetEvent[2];
        private Thread thread;
        private PullAudio pullAudio;
        private bool halted;
        private bool running;

        static DirectSound.WaveFormat ConvertWaveFormat(Qaryan.Audio.WaveFormat fmt)
        {
            DirectSound.WaveFormat result = new Microsoft.DirectX.DirectSound.WaveFormat();
            result.AverageBytesPerSecond = (int)fmt.AverageBytesPerSecond;
            result.BitsPerSample = (short)fmt.BitsPerSample;
            result.BlockAlign = (short)fmt.BlockAlign;
            result.Channels = (short)fmt.Channels;
            result.FormatTag = (DirectSound.WaveFormatTag)fmt.FormatTag;
            result.SamplesPerSecond = (int)fmt.SamplesPerSecond;
            return result;
        }

        [DllImport("user32.dll", SetLastError = false)]
        static extern IntPtr GetDesktopWindow();

        BufferDescription bufferDesc;

        public SoundPlayer(Control owner, PullAudio pullAudio, Qaryan.Audio.WaveFormat format)
        {
            this.pullAudio = pullAudio;

            this.soundDevice = new Device();
            this.soundDevice.SetCooperativeLevel(owner, CooperativeLevel.Normal);

            // Set up our wave format to 44,100Hz, with 16 bit resolution
            DirectSound.WaveFormat wf = ConvertWaveFormat(format);
//            wf.FormatTag = DirectSound.WaveFormatTag.Pcm;
//            wf.SamplesPerSecond = 44100;
//            wf.BitsPerSample = 16;
//            wf.Channels = channels;
//            wf.BlockAlign = (short)(wf.Channels * wf.BitsPerSample / 8);
//            wf.AverageBytesPerSecond = wf.SamplesPerSecond * wf.BlockAlign;

            this.samplesPerUpdate = 1024;

            // Create a buffer with 5 seconds of sample data
            bufferDesc = new BufferDescription(wf);
            bufferDesc.BufferBytes = this.samplesPerUpdate * wf.BlockAlign * 5;
            bufferDesc.ControlPositionNotify = true;
            bufferDesc.GlobalFocus = true;

            this.soundBuffer = new SecondaryBuffer(bufferDesc, this.soundDevice);

            Notify notify = new Notify(this.soundBuffer);

            fillEvent[0] = new AutoResetEvent(false);
            fillEvent[1] = new AutoResetEvent(false);

            // Set up two notification events, one at halfway, and one at the end of the buffer
            BufferPositionNotify[] posNotify = new BufferPositionNotify[2];
            posNotify[0] = new BufferPositionNotify();
            posNotify[0].Offset = bufferDesc.BufferBytes / 2 - 1;
            posNotify[0].EventNotifyHandle = fillEvent[0].SafeWaitHandle.DangerousGetHandle();
            posNotify[1] = new BufferPositionNotify();
            posNotify[1].Offset = bufferDesc.BufferBytes - 1;
            posNotify[1].EventNotifyHandle = fillEvent[1].SafeWaitHandle.DangerousGetHandle();

            notify.SetNotificationPositions(posNotify);

            this.thread = new Thread(new ThreadStart(SoundPlayback));
            this.thread.SetApartmentState(ApartmentState.STA);
            this.thread.Name = "SoundPlayback";
            this.thread.Priority = ThreadPriority.Highest;

            this.Pause();
            this.running = true;

            this.thread.Start();
        }

        public void Pause()
        {
            if (this.halted) return;

            this.halted = true;

            Monitor.Enter(this.thread);
        }

        public void Resume()
        {
            if (!this.halted) return;

            this.halted = false;

            Monitor.Pulse(this.thread);
            Monitor.Exit(this.thread);
        }

        bool stopped = false;

        public void Stop()
        {

            this.stopped = true;

        }

        public void Join()
        {
            if (!this.running)
                return;
            if (this.thread==null)
                return;
            thread.Join();
        }

        private void SoundPlayback()
        {
            stopped = false;
            int stoppedCountdown = 100;
            lock (this.thread)
            {
                if (!this.running) return;

                // Set up the initial sound buffer to be the full length
                int bufferLength = //(soundBuffer.Format.BitsPerSample/8)* this.samplesPerUpdate * 2 * soundBuffer.Format.Channels;
                            bufferDesc.BufferBytes;
                byte[] soundData = new byte[bufferLength];

                // Prime it with the first x seconds of data
                this.pullAudio(soundData, soundData.Length);
                this.soundBuffer.Write(0, soundData, LockFlag.None);

                // Start it playing
                this.soundBuffer.Play(0, BufferPlayFlags.Looping);

                int lastWritten = 0;
                while (this.running && (stoppedCountdown>0))
                {

                    if (this.halted)
                    {
                        Monitor.Pulse(this.thread);
                        Monitor.Wait(this.thread);
                    }

                    // Wait on one of the notification events
                    WaitHandle.WaitAny(this.fillEvent, 3, true);

                    // Get the current play position
                    int tmp = this.soundBuffer.PlayPosition;

                    // Generate new sounds from lastWritten to tmp in the sound buffer
                    if (tmp == lastWritten)
                    {
                        continue;
                    }
                    else
                    {
                        soundData = new byte [(tmp - lastWritten + bufferLength) % bufferLength];
                    }
                    if (stopped)
                        stoppedCountdown--;
                    if ((this.pullAudio!=null)&&!stopped)
                        this.pullAudio(soundData, soundData.Length);

                    // Write in the generated data
                    soundBuffer.Write(lastWritten, soundData, LockFlag.None);

                    // Save the position we were at
                    lastWritten = tmp;
                }
            }
            this.running = false;
        }

        public void Dispose()
        {
            this.running = false;
            this.Resume();

            if (this.soundBuffer != null)
            {
                this.soundBuffer.Dispose();
            }
            if (this.soundDevice != null)
            {
                this.soundDevice.Dispose();
            }
        }
    }
}