using BedrockSvrLog.Data;


namespace BedrockSvrLog;

public class DbHelpers
{
    private readonly AppDbContext MyAppDbContext;

    public DbHelpers(AppDbContext myAppDbContext)
    {
        MyAppDbContext = myAppDbContext;
    }
    public void addUserToDb(string name, string xuid, string pfid)
    {
        Program.writeToDebugFile($"Debug: Adding new user to database: Name: {name}, XUID: {xuid}, PFID: {pfid}");
        try
        {

            // Check if user already exists (by Xuid, which is [Key])
            var existingUser = MyAppDbContext.User.Find(xuid);
            if (existingUser == null)
            {
                var user = new User
                {
                    Name = name,
                    Xuid = xuid,
                    Pfid = pfid
                };
                MyAppDbContext.User.Add(user);
                MyAppDbContext.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            Program.writeToDebugFile($"Error adding user to DB: {ex}");
        }
    }
}
