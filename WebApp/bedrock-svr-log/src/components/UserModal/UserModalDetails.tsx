import { Box, Typography } from "@mui/material";
import { formatTimeCount } from "../../Helpers/timeHelper";
import { formatDateTime } from "../../Helpers/timeHelper";
import { type UserPlayerKills } from "../../Hooks/useGetUserPlayerKills";
import { type UserRealmEvent } from "../../Hooks/useGetUserRealmEvetns";
import RealmTable from "../RealmTable";
import { type Duration } from "../../Hooks/useGetDurations";

interface UserDetailsProps {
  selectedUser: Duration | null;
  userPlayerKills: UserPlayerKills | null;
  userRealmEvents: UserRealmEvent | null;
  userRealmEventsLoading: boolean;
}

const UserModalDetails = ({
  selectedUser,
  userPlayerKills,
  userRealmEvents,
  userRealmEventsLoading,
}: UserDetailsProps) => {
  const handleFavoriteKillEntity = (entity: UserPlayerKills | undefined) => {
    if (!entity) return "None";
    if (entity?.favouriteKillCount && entity?.favouriteKillCount > 0) {
      return `${
        entity?.favouriteKillEntity?.charAt(0).toUpperCase() +
          entity?.favouriteKillEntity?.slice(1).replace("_", " ") || "None"
      } with ${entity?.favouriteKillCount} kills.`;
    }
    return "None";
  };

  return (
    <Box
      className="flex flex-col items-start"
      sx={{ marginLeft: "2rem", flex: 4 }}
    >
      <Typography variant="h5" className="text-white mb-2">
        {selectedUser?.name}
      </Typography>
      <Typography variant="body2" className="text-gray-300">
        Total playtime:{" "}
        {formatTimeCount(selectedUser?.totalLiveDuration ?? "0")}
      </Typography>
      <Typography
        variant="body2"
        className="text-gray-300"
        sx={{ marginBottom: "0.5rem" }}
      >
        Last online: {formatDateTime(selectedUser?.spawnTime ?? "")}
      </Typography>
      <Typography variant="body2" className="text-gray-300">
        Total kills: {userPlayerKills?.totalKills ?? "0"}
      </Typography>
      <Typography variant="body2" className="text-gray-300">
        Most kills: {handleFavoriteKillEntity(userPlayerKills ?? undefined)}
      </Typography>
      {userRealmEvents && userRealmEvents.hasRealmEvents && (
        <Box sx={{ width: "90%" }}>
          <Typography
            variant="h6"
            className="text-white text-left"
            sx={{ marginTop: "1.5rem" }}
          >
            Recent Achievements
          </Typography>
          <RealmTable
            data={userRealmEvents.realmEvents}
            isLoading={userRealmEventsLoading}
            showAvatar={false}
            showUsername={false}
            showHeader={false}
            isInUserModal={true}
          />
        </Box>
      )}
    </Box>
  );
};

export default UserModalDetails;
