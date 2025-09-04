import {
  Modal,
  Box,
  Typography,
  IconButton,
  Avatar,
  Fade,
} from "@mui/material";
import { CheckCircle, Close, Close as CloseIcon } from "@mui/icons-material";
import { type Duration } from "../Hooks/useGetDurations";
import { formatDateTime, formatTimeCount } from "../Helpers/timeHelper";
import { useGetUserRealmEvents } from "../Hooks/useGetUserRealmEvetns";
import RealmTable from "./RealmTable";
import {
  useGetUserPlayerKills,
  type UserPlayerKills,
} from "../Hooks/useGetUserPlayerKills";

interface UserModalProps {
  selectedUser: Duration | null;
  handleModalClose: () => void;
  modalOpen: boolean;
}

const UserModal = ({
  selectedUser,
  handleModalClose,
  modalOpen,
}: UserModalProps) => {
  const { data: userRealmEvents, isLoading: userRealmEventsLoading } =
    useGetUserRealmEvents(selectedUser?.xuid ?? 0, modalOpen);

  const { data: userPlayerKills } = useGetUserPlayerKills(
    selectedUser?.xuid ?? 0,
    modalOpen
  );

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
    <Modal
      open={modalOpen}
      onClose={handleModalClose}
      closeAfterTransition
      className="flex items-center justify-center"
    >
      <Fade in={modalOpen}>
        <Box
          className="flex flex-col w-1/2 border border-gray-600 min-w-[700px]"
          sx={{
            backgroundColor: "#1f2937",
            color: "white",
            paddingInline: "1rem",
            paddingTop: "0.5rem",
            paddingBottom: "2rem",
            maxWidth: "600px",
          }}
        >
          <Box className="flex justify-between items-center mb-4">
            <Typography
              variant="h6"
              className="text-white"
              sx={{ marginLeft: "0.8rem" }}
            >
              User Details
            </Typography>
            <IconButton
              onClick={handleModalClose}
              className="text-gray-400 hover:text-white"
            >
              <CloseIcon />
            </IconButton>
          </Box>

          {selectedUser && (
            <Box className="text-center flex flex-row">
              <Box
                sx={{
                  display: "flex",
                  flex: 1,
                  alignItems: "center",
                  justifyContent: "center",
                  height: "100%",
                }}
              >
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
                    src={selectedUser.diceBearAvatarUrl}
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
                      {handlePlayerClass(userPlayerKills)}
                    </Typography>
                    <Typography
                      variant="body2"
                      className="text-gray-300 flex flex-row items-center justify-center"
                      sx={{ gap: "0.5rem" }}
                    >
                      <span className="text-gray-300">Online: </span>
                      {selectedUser.isOnline ? (
                        <CheckCircle
                          sx={{ color: "#22c55e", fontSize: "1.2rem" }}
                        />
                      ) : (
                        <Close sx={{ color: "#ef4444", fontSize: "1.2rem" }} />
                      )}
                    </Typography>
                  </Box>
                </Box>
              </Box>
              <Box
                className="flex flex-col items-start"
                sx={{ marginLeft: "2rem", flex: 4 }}
              >
                <Typography variant="h5" className="text-white mb-2">
                  {selectedUser.name}
                </Typography>
                <Typography variant="body2" className="text-gray-300">
                  Total playtime:{" "}
                  {formatTimeCount(selectedUser.totalLiveDuration)}
                </Typography>
                <Typography
                  variant="body2"
                  className="text-gray-300"
                  sx={{ marginBottom: "0.5rem" }}
                >
                  Last online: {formatDateTime(selectedUser.spawnTime)}
                </Typography>
                <Typography variant="body2" className="text-gray-300">
                  Total kills: {userPlayerKills?.totalKills ?? "0"}
                </Typography>
                <Typography variant="body2" className="text-gray-300">
                  Most kills: {handleFavoriteKillEntity(userPlayerKills)}
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
                    />
                  </Box>
                )}
              </Box>
            </Box>
          )}
        </Box>
      </Fade>
    </Modal>
  );
};

export default UserModal;
