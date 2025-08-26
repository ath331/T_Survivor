pipeline {
  agent any

  stages {
    stage('Build Solution') {
      steps {
        bat """
	cd AtServerEngine
	"C:\\Program Files\\Microsoft Visual Studio\\2022\\BuildTools\\MSBuild\\Current\\Bin\\MSBuild.exe" Server.sln -m -p:Configuration=Release -p:Platform=x64 -v:diag
	"""
      }
    }
  }
}