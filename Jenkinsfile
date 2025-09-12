pipeline {
  agent any

  stages {
    stage('Build Projects') {
      steps {
        bat """
        set MSB="C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\amd64\\MSBuild.exe"

        %MSB% AtServerEngine\\GameServer\\GameServer.vcxproj -p:Configuration=Release -p:Platform=x64
        %MSB% AtServerEngine\\ServerCore\\ServerCore.vcxproj -p:Configuration=Release -p:Platform=x64
        %MSB% AtServerEngine\\Network\\Network.vcxproj -p:Configuration=Release -p:Platform=x64
        %MSB% AtServerEngine\\Public\\Public.vcxproj -p:Configuration=Release -p:Platform=x64
        %MSB% AtServerEngine\\TitleServer\\TitleServer.vcxproj -p:Configuration=Release -p:Platform=x64
        """
      }
    }
  }
}