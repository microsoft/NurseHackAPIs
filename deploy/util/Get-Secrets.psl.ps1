$kvname="NurseHack4HealthKeyVault";
$kvexport=@{};
$secrets=Get-AzKeyVaultSecret -VaultName $kvname;
foreach($s in $secrets){
    if($s.Enabled){
        $value=Get-AzKeyVaultSecret -VaultName $kvname -Name $s.Name -AsPlainText;
        $kvexport.Add($s.Name,$value);
    }
}
$kvexport | ConvertTo-Json;