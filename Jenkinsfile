pipeline {
  agent any

  stages {
    stage('Check Files') {
      steps {
        bat """
        echo 현재 폴더: %CD%
        dir
        dir AtServerEngine
        """
      }
    }

    stage('Build Solution') {
      steps {
        bat """
        "C:\\Program Files\\Microsoft Visual Studio\\2022\\BuildTools\\MSBuild\\Current\\Bin\\MSBuild.exe" "AtServerEngine\\Server.sln" -m -p:Configuration=Release -p:Platform=x64
        """
      }
    }
  }
}