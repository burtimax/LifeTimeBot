### Команды для миграций
```bash
dotnet ef migrations add AddUserTaskNotificationEntity -o Db/AppDb/Migrations --context AppDbContext --project LifeTimeBot/LifeTimeBot --startup-project LifeTimeBot/LifeTimeBot

dotnet ef database update AddUserTaskEntity --context AppDbContext --project LifeTimeBot/LifeTimeBot --startup-project LifeTimeBot/LifeTimeBot

dotnet ef migrations remove --context AppDbContext --project LifeTimeBot/LifeTimeBot --startup-project LifeTimeBot/LifeTimeBot

```