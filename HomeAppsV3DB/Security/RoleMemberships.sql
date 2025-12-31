ALTER ROLE [db_owner] ADD MEMBER [IIS APPPOOL\No Managed Code];


GO
ALTER ROLE [db_owner] ADD MEMBER [NT AUTHORITY\NETWORK SERVICE];


GO
ALTER ROLE [db_owner] ADD MEMBER [NT AUTHORITY\Local account];


GO
ALTER ROLE [db_datawriter] ADD MEMBER [IIS APPPOOL\No Managed Code];


GO
ALTER ROLE [db_datareader] ADD MEMBER [IIS APPPOOL\No Managed Code];

