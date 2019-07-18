---
external help file: Microsoft.PowerBI.Commands.OnPremisesDataGateway.dll-Help.xml
Module Name: MicrosoftPowerBIMgmt.OnPremisesDataGateway
online version:
schema: 2.0.0
---

# Add-OnPremisesDataGatewayClusterDatasourceUser

## SYNOPSIS
Grants the permissions required to use the specified datasource for the specified user

## SYNTAX

```
Add-OnPremisesDataGatewayClusterDatasourceUser [-Scope <PowerBIUserScope>] -GatewayClusterId <Guid>
 -GatewayClusterDatasourceId <Guid> -DatasourceUserAccessRight <DatasourceUserAccessRight>
 [-DisplayName <String>] -UserEmailAddress <String> [<CommonParameters>]
```

## DESCRIPTION
Grants the permissions required to use the specified datasource for the specified user

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-OnPremisesDataGatewayClusterDatasourceStatus -GatewayClusterId DC8F2C49-5731-4B27-966B-3DB5094C2E77 -GatewayClusterDatasourceId 64C574B7-86C6-4560-B710-40AC18990804 -DatasourceUserAccessRight Read -UserEmailAddress testEmail@tenant.com
```

Grants read access to 'testUpn@tenant.com' for the datasource.

## PARAMETERS

### -DatasourceUserAccessRight
{{Fill DatasourceUserAccessRight Description}}

```yaml
Type: DatasourceUserAccessRight
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DisplayName
{{Fill DisplayName Description}}

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -GatewayClusterDatasourceId
{{Fill GatewayClusterDatasourceId Description}}

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: DatasourceId, Datasource

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -GatewayClusterId
{{Fill GatewayClusterId Description}}

```yaml
Type: Guid
Parameter Sets: (All)
Aliases: Cluster, Id

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Scope
{{Fill Scope Description}}

```yaml
Type: PowerBIUserScope
Parameter Sets: (All)
Aliases:
Accepted values: Individual, Organization

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UserEmailAddress
{{Fill UserEmailAddress Description}}

```yaml
Type: String
Parameter Sets: (All)
Aliases: User, EmailAddress

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Void

## NOTES

## RELATED LINKS