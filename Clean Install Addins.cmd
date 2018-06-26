echo off
"c:\Program Files\Common Files\Microsoft Shared\VSTO\10.0\VSTOInstaller.exe" /Uninstall "%CD%\Word\XlantWord.vsto"
"c:\Program Files\Common Files\Microsoft Shared\VSTO\10.0\VSTOInstaller.exe" /Uninstall "%CD%\Outlook\XlantOutlook.vsto"
echo Clear cache
rundll32 dfshim CleanOnlineAppCache
echo Uninstallation complete.
TIMEOUT /T 5
echo Install Word Addin
"C:\Program Files\Common Files\microsoft shared\VSTO\10.0\VSTOInstaller.exe" /i "%CD%\Word\XlantWord.vsto"
echo Word Installation Complete.
echo Install Outlook Addin
"C:\Program Files\Common Files\microsoft shared\VSTO\10.0\VSTOInstaller.exe" /i "%CD%\Outlook\XlantOutlook.vsto"
echo Outlook Installation Complete.
TIMEOUT /T 5
exit

