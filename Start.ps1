$server = "localhost"
$database = "Nhea"

$user = "sa"
$password = "12Aa2sr13cx"

$containerName = "sqltest"

docker pull mcr.microsoft.com/mssql/server
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 --name $containerName -d mcr.microsoft.com/mssql/server:latest

Start-Sleep -s 3

docker exec -it $containerName /opt/mssql-tools/bin/sqlcmd -S $server -U $user -P $password -Q "CREATE DATABASE $database;"

$logsScript = ""
Get-Content ".\SQL\nhea_Log.sql"| foreach {
 $logsScript = $logsScript + $_
}

$mailQueueScript = ""
Get-Content ".\SQL\nhea_MailQueue.sql"| foreach {
 $mailQueueScript = $mailQueueScript + $_
}

$Script = $logsScript + $mailQueueScript

$batches = $Script -split "GO"

foreach($batch in $batches)
{
    if ($batch.Trim() -ne ""){
		Write-Output $batch
		docker exec -it $containerName /opt/mssql-tools/bin/sqlcmd -S $server -U $user -P $password -Q "USE [$database];$batch"
    }
}