if not exist "TCM.Web\wwwroot" mkdir TCM.Web\wwwroot
del /f /s /q TCM.Web\wwwroot\*.* > NUL
xcopy /q /e react\build\*.* TCM.Web\wwwroot
dir TCM.Web\wwwroot
