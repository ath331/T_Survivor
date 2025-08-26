pipeline {
  agent any
  stages {
    stage('Checkout') {
      steps {
        // dev 브랜치에서 소스 가져오기
        git branch: 'dev', credentialsId: 'TSurvivor', url: 'https://github.com/ath331/T_Survivor.git'
      }
    }

    stage('Build Solution') {
      steps {
        bat '''
        :: MSBuild 경로 찾기
        for /f "usebackq delims=" %%i in (`"%ProgramFiles(x86)%\\Microsoft Visual Studio\\Installer\\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\\**\\Bin\\MSBuild.exe"`) do set MSB=%%i

        :: Release 빌드 실행
        "%MSB%" Server.sln -m -p:Configuration=Release -p:Platform=x64
        '''
      }
    }
  }
}