rem
rem Build CQuery Language Server
rem https://github.com/cquery-project/cquery-project/cquery
rem
rem Prerequisites
rem - Microsoft C++ tools installed (part of VS 2017 community)
rem - Python installed and in %PATH%
rem - 7z.exe installed and in %PATH%
rem - CQuery third party parts are copied in respective folder

python waf configure
python waf build