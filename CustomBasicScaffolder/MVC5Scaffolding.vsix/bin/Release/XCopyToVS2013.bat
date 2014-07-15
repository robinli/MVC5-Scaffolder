set  toPath=C:\Users\Robin\AppData\Local\Microsoft\VisualStudio\12.0\Extensions\ay5otj3q.e53

set  fromPath=E:\GitHub\MVC5-Scaffolder\CustomBasicScaffolder\MVC5Scaffolding.vsix\bin\Debug

xcopy %fromPath%\*.dll %toPath%\ /s/d/y

xcopy %fromPath%\Templates\*.t4 %toPath%\Templates\ /s/d/y