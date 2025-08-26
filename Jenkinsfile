stage('Build Projects') {
  steps {
    bat """
    cd AtServerEngine

    set MSB="C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\amd64\\MSBuild.exe"

    %MSB% ServerCore\\ServerCore.vcxproj -p:Configuration=Release -p:Platform=x64
    %MSB% Network\\Network.vcxproj -p:Configuration=Release -p:Platform=x64
    %MSB% Public\\Public.vcxproj -p:Configuration=Release -p:Platform=x64
    %MSB% GameServer\\GameServer.vcxproj -p:Configuration=Release -p:Platform=x64
    %MSB% TitleServer\\TitleServer.vcxproj -p:Configuration=Release -p:Platform=x64
    """
  }
}
