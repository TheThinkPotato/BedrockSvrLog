import { Box, Typography } from "@mui/material";
import { formatDateTime, formatTimeCount } from "../../Helpers/timeHelper";
import { type UserPlayerKills } from "../../Hooks/useGetUserPlayerKills";
import { type UserRealmEvent } from "../../Hooks/useGetUserRealmEvents";
import RealmTable from "../Tables/RealmTable";
import { useGetUserDurations } from "../../Hooks/useGetUserDurations";
import { creatureNameCleanUp } from "../../Helpers/creatureHelper";

interface UserDetailsProps {
  selectedUserXuid: number;
  userPlayerKills: UserPlayerKills | null;
  userRealmEvents: UserRealmEvent | null;
  userRealmEventsLoading: boolean;
}

const UserModalDetails = ({
  selectedUserXuid,
  userPlayerKills,
  userRealmEvents,
  userRealmEventsLoading,
}: UserDetailsProps) => {
  const handleFavoriteKillEntity = (entity: UserPlayerKills | undefined) => {
    if (!entity) return "None";
    if (entity?.favouriteKillCount && entity?.favouriteKillCount > 0) {
      return `${
        creatureNameCleanUp(entity?.favouriteKillEntity ?? "") || "None"
      } with ${entity?.favouriteKillCount} kills.`;
    }
    return "None";
  };

  const { data: userDurations } = useGetUserDurations(selectedUserXuid);
  
  return (
    <Box
      className="flex flex-col items-start"
      sx={{ marginLeft: "2rem", flex: 4 }}
    >
      <Typography variant="h5" className="text-white mb-2">
        {userDurations?.name}
      </Typography>
      <Typography variant="body2" className="text-gray-300">
        Total playtime:{" "}        
        {formatTimeCount(userDurations?.totalLiveDuration ?? "0")}
      </Typography>
      <Typography
        variant="body2"
        className="text-gray-300"
        sx={{ marginBottom: "0.5rem" }}
      >
        Last online: {formatDateTime(userDurations?.spawnTime ?? "")}
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
