@echo off
if %processor_architecture%==x86 (
cd /d %~dp0
copy dsoframer.ocx %windir%\system32\ 
%windir%\system32\regsvr32.exe %windir%\system32\dsoframer.ocx 
) else (
cd /d %~dp0
copy dsoframer.ocx %windir%\syswow64\ 
%windir%\syswow64\regsvr32.exe %windir%\syswow64\dsoframer.ocx 
)
pause>nul