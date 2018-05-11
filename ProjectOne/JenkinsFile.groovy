node('master') {
    stage('import') {
        try {
            git 'https://github.com/1804-Apr-USFdotnet/ram-adhikari-project1.git'
        }
        catch (exc) {
            slackError('import')
            throw exc
        }
    }
    stage('build') {
        try {
            dir('ram-adhikari-project1/ProjectOne') {
                bat 'nuget restore'
                bat 'msbuild /p:MvcBuildViews=true'
            }
        }
        catch (exc) {
            slackError('build')
            throw exc
        }
    }
    stage('test') {
        try {
            dir('ram-adhikari-project1/ProjectOne') {
            }
        }
        catch (exc) {
            slackError('test')
            throw exc
        }
    }
    stage('analyze') {
        try {
            dir('ram-adhikari-project1/ProjectOne') {
            }
        }
        catch (exc) {
            slackError('analyze')
            throw exc
        }
    }
    stage('package') {
        try {
            dir('ram-adhikari-project1/ProjectOne') {
                bat 'msbuild ProjectOne.PL /t:package /p:Configuration=Debug;PackageFileName=..\\Package.zip'
            }
        }
        catch (exc) {
            slackError('package')
            throw exc
        }
    }
    stage('deploy') {
        try {
            dir('ram-adhikari-project1/ProjectOne') {
                bat "msdeploy -verb:sync -source:package=\"%CD%\\Package.zip\" -dest:auto,computerName=\"https://ec2-18-191-64-122.us-east-2.compute.amazonaws.com:8172/msdeploy.axd\",userName=\"Administrator\",password=\"${env.Deploy_Password}\",authtype=\"basic\",includeAcls=\"False\" -disableLink:AppPoolExtension -disableLink:ContentExtension -disableLink:CertificateExtension -setParam:\"IIS Web Application Name\"=\"Default Web Site/ProjectOne\" -enableRule:AppOffline -allowUntrusted"
            }
        }
        catch (exc) {
            slackError('deploy')
            throw exc
        }
    }
}

def slackError(stageName) {
    slackSend color: 'danger', message: "FAILED ${stageName} stage: [<${JOB_URL}|${env.JOB_NAME}> <${env.BUILD_URL}console|${env.BUILD_DISPLAY_NAME}>] [${currentBuild.durationString.replace(' and counting', '')}]"
}
      