### Команды для миграций
```bash
dotnet ef migrations add AddEmojiToActivity -o Db/AppDb/Migrations --context AppDbContext --project LifeTimeBot/LifeTimeBot --startup-project LifeTimeBot/LifeTimeBot

dotnet ef database update --context AppDbContext --project MultipleTestBot/MultipleTestBot --startup-project MultipleTestBot/MultipleTestBot

dotnet ef migrations remove --context AppDbContext --project MultipleTestBot/MultipleTestBot --startup-project MultipleTestBot/MultipleTestBot

```