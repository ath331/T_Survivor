pipeline {
  agent any

  stages {
    stage('Build Solution') {
      steps {
        bat """
        :: MSBuild 경로 찾기
        "C:\\Program Files\\Microsoft Visual Studio\\2022\\BuildTools\\MSBuild\\Current\\Bin\\MSBuild.exe" "C:\\ProgramData\\Jenkins\\.jenkins\\workspace\\T_Survivor_Build\\AtServerEngine\\Server.sln" -m -p:Configuration=Release -p:Platform=x64
        """
      }
    }
  }
}