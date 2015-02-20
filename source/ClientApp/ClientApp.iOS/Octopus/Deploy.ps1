
$run_location = "`~/OctopusTentacle/#{Octopus.Release.Number}/"

Write-Output "Transferring Payload to #{MacHostName}"
#{nixTools}\ssh.exe -n -i "#{MacSSHKey}" #{MacUser}@#{MacHostName} "mkdir -p $run_location"

#{nixTools}\scp.exe -i "#{MacSSHKey}" *.sh #{MacUser}@#{MacHostName}:$run_location
#{nixTools}\scp.exe -i "#{MacSSHKey}" *.ipa #{MacUser}@#{MacHostName}:$run_location
#{nixTools}\scp.exe -i "#{MacSSHKey}" Deliverfile #{MacUser}@#{MacHostName}:$run_location
#{nixTools}\scp.exe -r -i "#{MacSSHKey}" deliver #{MacUser}@#{MacHostName}:$run_location

Write-Output "Executing Deploy.sh"
#{nixTools}\ssh.exe -n -i "#{MacSSHKey}" #{MacUser}@#{MacHostName} "cd $run_location; cat Deploy.sh | col -b > Deploy2.sh; mv -f Deploy2.sh Deploy.sh; chmod +x Deploy.sh; ./Deploy.sh;"
$result = $LastExitCode

Write-Output "Cleaning up Payload"
#{nixTools}\ssh.exe -n -i "#{MacSSHKey}" #{MacUser}@#{MacHostName} "rm -r $run_location"

exit $result
