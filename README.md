# 🗄️ Mongolia
![GitHub Workflow Status (branch)](https://img.shields.io/github/workflow/status/EpicTestingTempOrganizationForStuff/Mongolia/.NET/release?label=Release%20Build&style=for-the-badge) ![GitHub Workflow Status (branch)](https://img.shields.io/github/workflow/status/EpicTestingTempOrganizationForStuff/Mongolia/publish%20to%20nuget/release?color=004880&label=Nuget%20Publish&style=for-the-badge)

## 👷 Installation
[Nuget](https://www.nuget.org/packages/Mongolia)

[Releases](https://github.com/EpicTestingTempOrganizationForStuff/Mongolia/releases)

## 🕴️ Usage

### Create repo
```c#
using Mongolia;

//Create a new database
DB db = new DB("mongodb://localhost:27017/?ssl=false", "mongoliaExample");

//Get repo from context
DbRepository<User> userRepo = db.GetRepository<User>();
```

### Create
```c#
User newUser = new User() {
    Username = "Test User"
};
//Await the creation of the newUser
await userRepo.Create(newUser);
```

### Find One
```c#
User user = await UserRepo.FindOne(new { Username = "Test user" });
```

### Update
```c#
User newUser = new User() {
    Username = "Test User"
};

//Await the creation of the newUser
User user = await userRepo.Create(newUser);

user.Username = "Not Test User";
await user.Save();
```

## 🥅 Goals
  * [ ] Usable

## ✨ Contributors

<table>
  <tr>
    <td align="center"><a href="https://ahowe.dev/"><img src="https://avatars2.githubusercontent.com/u/16884313?v=4" width="100px;" alt=""/><br /><sub><b>unlimitedcoder2</b></sub></a><br /><a href="https://github.com/unlimitedcoder2/dbrepo/commits?author=unlimitedcoder2" title="Code">💻</a></td>
       <td align="center"><a href="https://mwareing.xyz/"><img src="https://avatars1.githubusercontent.com/u/29664925?s=460&v=4" width="100px;" alt=""/><br /><sub><b>TatoExp</b></sub></a><br /><a href="https://github.com/unlimitedcoder2/dbrepo/commits?author=TatoExp" title="Code">💻</a></td>
  </tr>
</table>
