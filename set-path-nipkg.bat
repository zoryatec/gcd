for /f "usebackq tokens=2,*" %%A in (`reg query HKCU\Environment /v PATH`) do set my_user_path=%%B
setx PATH "C:\Program Files\National Instruments\NI Package Manager;%my_user_path%"