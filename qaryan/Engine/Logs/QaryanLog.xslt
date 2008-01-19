<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" encoding="UTF-8"
doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN"
doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
  <xsl:template match="/Log">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <style>
          body {
          font-family:Arial;
          font-size:14px;
          }


          .Debug {}
          .Info {}
          .MajorInfo {}
          .Warning {}
          .Error {}

          .Debug td.message {
          color: #555555
          }


          .Info td.message {
          color: brown
          }

          .MajorInfo td.message {
          color: blue
          }

          .Warning td.message {
          font-weight:bold;
          color: blue
          }

          .Error td.message {
          font-weight:bold;
          color: red;
          }

          .component0 {
          background-color:#E0FFFF;
          }

          .component1 {
          background-color:#E0FF9F;
          }

          .component2 {
          background-color:#FFE7BF;
          }

          .component3 {
          background-color:#FFBBBB;
          }

          .component4 {
          background-color:#FCFFCD;
          }

          .component5 {
          background-color:#D8DFD1;
          }

          .component6 {
          background-color:#C2D7EF;
          }

          .component7 {
          background-color:#EFDB00;
          }
        </style>
        <script language="JavaScript" type="text/javascript" src="resx://QaryanLog_js"></script>
        <title>Qaryan TTS Synthesis Log</title>
      </head>
      <body>
        <h2>
          Qaryan TTS Synthesis Log
        </h2>
        Detail level:
        <select onchange="filter(this)">
          <option value="1" selected="selected">Debug</option>
          <option value="2">Info</option>
          <option value="4">Major info</option>
          <option value="8">Warning</option>
          <option value="16">Error</option>
        </select>
        <table>
          <tr style="font-weight:bold">
            <td>
              Time
            </td>
            <td>
              Source
            </td>
            <td>
              Message
            </td>
          </tr>
          <xsl:for-each select="LogLine">
            <tr>
              <xsl:attribute name="class">
                logline
                <xsl:value-of select="@TextLevel"/>
                component<xsl:value-of select="@ComponentNum"/>
              </xsl:attribute>
              <td>
                <xsl:value-of select="@Time" />
              </td>
              <td>
                <xsl:value-of select="@Component" />
              </td>
              <td class="message">
                <xsl:value-of select="current()" />
              </td>
            </tr>


          </xsl:for-each>
        </table>
      </body>




    </html>
  </xsl:template>
</xsl:stylesheet>