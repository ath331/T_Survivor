pipeline {
  agent any

    stage('Build Solution') {
      steps {
        bat '''
        :: MSBuild 경로 찾기
        for /f "usebackq delims=" %%i in (`"%ProgramFiles(x86)%\\Microsoft Visual Studio\\Installer\\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\\**\\Bin\\MSBuild.exe"`) do set MSB=%%i

        :: Release 빌드 실행
        "%MSB%" AtServerEngine\\Server.sln -m -p:Configuration=Release -p:Platform=x64
        '''
      }
    }
  }
}