#New-AzKeyVault -VaultName HackKV -ResourceGroupName rg-hackathon-starter-kit -Location 'Central US' -EnabledForTemplateDeployment
$kvname="HackKV"
$secretsimport=Get-Content ./infile.json | ConvertFrom-Json -AsHashtable -Depth 1;
foreach ($s in $secretsimport.keys){
    if($s){
        $val=$secretsimport[$s];
        $secretvalue = ConvertTo-SecureString $val -AsPlainText -Force
        Set-AzKeyVaultSecret -VaultName $kvname -Name $s -SecretValue $secretvalue
        Write-Output $"Added {$s}";
    }
}