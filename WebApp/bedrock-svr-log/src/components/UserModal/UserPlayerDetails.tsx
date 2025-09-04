import { Avatar, Box } from "@mui/material"
import { Typography } from "@mui/material"
import type { UserPlayerKills } from "../../Hooks/useGetUserPlayerKills";
import { CheckCircle, Close } from "@mui/icons-material";
import { type Duration } from "../../Hooks/useGetDurations";


interface UserPlayerDetailsProps {
    selectedUser: Duration | null;
    userPlayerKills: UserPlayerKills | null;    
}

const UserPlayerDetails = ({ selectedUser, userPlayerKills }: UserPlayerDetailsProps) => {

    const handlePlayerClass = (entity: UserPlayerKills | undefined) => {
        if (!entity || entity?.favouriteKillCount === 0) return "Human";
        if (entity?.favouriteKillCount) {
          return `${
            entity?.favouriteKillEntity?.charAt(0).toUpperCase() +
            entity?.favouriteKillEntity?.slice(1).replace("_", " ")
          } slayer`;
        }
        return "Human";
      };


    return (
        <Box
        sx={{
          height: "100%",
          display: "flex",
          flexDirection: "column",
          justifyContent: "end",
          width: "100%",
          alignItems: "center",
        }}
      >
        <Avatar
          src={selectedUser?.diceBearAvatarUrl}
          sx={{
            width: 80,
            height: 80,
            marginTop: "1rem",
            marginBottom: "0.5rem",
          }}
        />
        <Box className="flex flex-col items-center justify-center">
          <Typography
            variant="body2"
            className="text-gray-300"
            sx={{
              maxWidth: "120px",
              marginBottom: "0.5rem",
              textAlign: "center",
            }}
          >
            {handlePlayerClass(userPlayerKills ?? undefined)}
          </Typography>
          <Typography
            variant="body2"
            className="text-gray-300 flex flex-row items-center justify-center"
            sx={{ gap: "0.5rem" }}
          >
            <span className="text-gray-300">Online: </span>
            {selectedUser?.isOnline ? (
              <CheckCircle
                sx={{ color: "#22c55e", fontSize: "1.2rem" }}
              />
            ) : (
              <Close sx={{ color: "#ef4444", fontSize: "1.2rem" }} />
            )}
          </Typography>
        </Box>
      </Box>
    )
}

export default UserPlayerDetails;