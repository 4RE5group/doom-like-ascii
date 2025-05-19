@echo off

del /f doom-like.exe

call C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe -out:doom-like.exe *.cs & pause

cls

call doom-like.exe

pause