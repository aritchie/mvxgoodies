@echo off
del *.nupkg
nuget pack MvvmCross.Autofac.Interop.nuspec
nuget pack MvvmCross.ReactiveUI.Interop.nuspec
pause