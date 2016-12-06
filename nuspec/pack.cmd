@echo off
del *.nupkg
rem nuget pack MvvmCross.Autofac.Interop.nuspec
nuget pack MvvmCross.ReactiveUI.Interop.nuspec
pause