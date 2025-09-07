using HealthFitnessAPI.Constants;
using HealthFitnessAPI.Constants.Enums;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Extensions;
using HealthFitnessAPI.Helpers;
using HealthFitnessAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HealthFitnessAPI.Services.Init;

public interface IMockDataInitService
{
    Task Initialize();
}

public class MockDataInitService(IUnitOfWork unitOfWork, IFileService fileService) : IMockDataInitService
{
    public async Task Initialize()
    {
        List<string> maleNames = ["Koppány", "Kristóf", "Gábor", "Patrik", "Bence", "Máté", "Zsolt", "Adrián"];
        List<string> femaleNames = ["Zsófia", "Ildikó", "Emese", "Luca", "Réka", "Petra", "Szofi", "Rebeka", "Emma"];
        List<string> lastNames = ["Kovács", "Takács", "Szabó", "Molnár", "Lakatos", "Teknővájó"];
        const string userPw = "test01";

        List<string> achievementCategories = ["Fekvenyomás", "Gugolás", "Lehúzás", "Felülés", "Fekvőtámasz"];
        List<string> achievementLevelNames = ["Bronz", "Ezüst", "Arany", "Platina"];

        for (var i = 0; i < 20; i++)
        {
            var name = maleNames.GetRandom();
            var lastName = lastNames.GetRandom();

            await unitOfWork.GetRepository<User>().CreateAsync(new User
            {
                DisplayName = $"{lastName} {name}",
                FullName = $"{lastName} {name}",
                Username = $"{lastName}.{name}{i}",
                Email = $"{lastName}.{name}{i}@gmail.com",
                Gender = Gender.Man,
                Role = Roles.User,
                Password = Hash.HashPassword(userPw)
            });
        }

        for (var i = 0; i < 20; i++)
        {
            var name = femaleNames.GetRandom();
            var lastName = lastNames.GetRandom();

            await unitOfWork.GetRepository<User>().CreateAsync(new User
            {
                DisplayName = $"{lastName} {name}",
                FullName = $"{lastName} {name}",
                Username = $"{lastName}.{name}{i}",
                Email = $"{lastName}.{name}{i}@gmail.com",
                Gender = Gender.Woman,
                Role = Roles.User,
                Password = Hash.HashPassword(userPw)
            });
        }

        foreach (var achievementLevel in achievementLevelNames)
            await unitOfWork.GetRepository<AchievementLevel>().CreateAsync(new AchievementLevel
            {
                Name = achievementLevel
            });

        await unitOfWork.SaveChangesAsync();

        var achievementLevels = await unitOfWork.GetRepository<AchievementLevel>().GetAllAsync();
        var image = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "InitFiles",
            "badge.txt"));
        List<string> thresholdTypes = ["kg", "db"];

        foreach (var achievement in achievementCategories)
        {
            var achievementToSave = new Achievement
            {
                Category = achievement,
                Description = $"Csinálj 10 {achievement} gyakorlatot",
                AchievementLevelThresholds =
                [
                ]
            };

            foreach (var achievementLevel in achievementLevels)
                achievementToSave.AchievementLevelThresholds.Add(
                    new AchievementLevelThreshold
                    {
                        AchievementLevelId = achievementLevel.Id,
                        MaleThreshold = new Random().Next(0, 20),
                        FemaleThreshold = new Random().Next(0, 10),
                        ThresholdType = thresholdTypes.GetRandom(),
                        Order = achievementLevels.IndexOf(achievementLevel),
                        LogoPath = await fileService.SaveBase64PngAsync(image,
                            $"{achievementToSave.Category}_{achievementLevel.Id}.png")
                    });

            await unitOfWork.GetRepository<Achievement>().CreateAsync(achievementToSave);
        }

        await unitOfWork.SaveChangesAsync();

        var users = unitOfWork.GetRepository<User>().GetAllAsQueryable(true).Include(u => u.FriendsRecieved)
            .Include(u => u.FriendsSent).ToList();

        foreach (var user in users)
        foreach (var friend in users.Where(u => u.Id != user.Id).Take(users.Count / 2).ToList())
        {
            if (user.FriendsSent.Any(f => f.FriendId == friend.Id) ||
                user.FriendsRecieved.Any(f => f.UserId == friend.Id)) continue;

            user.FriendsSent.Add(new Friendship
            {
                FriendId = friend.Id,
                UserId = user.Id,
                LastUpdated = DateTime.UtcNow.AddDays(new Random().Next(-5, 5)),
                Status = (FriendshipStatus)new Random().Next(0, 2)
            });

            await unitOfWork.SaveChangesAsync();
        }

        var achievements = await unitOfWork.GetRepository<Achievement>().GetAllAsync();

        foreach (var achievement in achievements)
        {
            var randomUsers = users.Shuffle().Take(users.Count / 2).ToList();

            var userAchievements = randomUsers.Select(u => new UserAchievement
            {
                UserId = u.Id,
                AchievementId = achievement.Id,
                AchievementLevelId = achievementLevels.GetRandom().Id,
                CreatedAt = DateTime.UtcNow.AddDays(new Random().Next(-5, 5))
            });

            await unitOfWork.GetRepository<UserAchievement>().CreateRangeAsync(userAchievements);
        }

        await unitOfWork.SaveChangesAsync();
    }
}