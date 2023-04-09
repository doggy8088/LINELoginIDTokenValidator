```sh
dotnet new console -n LINELoginIDTokenValidator
cd LINELoginIDTokenValidator

dotnet new gitignore

git init
git add .
git commit -m "Initial commit"

dotnet add package System.CommandLine --prerelease

git add .
git commit -m "Add System.CommandLine package"

```