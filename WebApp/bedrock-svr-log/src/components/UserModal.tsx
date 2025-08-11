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
  console.log(userRealmEvents, userRealmEventsLoading);

  return (
    <Modal
      open={modalOpen}
      onClose={handleModalClose}
      closeAfterTransition
      className="flex items-center justify-center"
    >
      <Fade in={modalOpen}>
        <Box
          className="flex flex-col w-1/2 mx-4 border border-gray-600"
          sx={{
            backgroundColor: "#1f2937",
            color: "white",
            paddingInline: "1rem",
            paddingTop: "0.5rem",
            paddingBottom: "1rem",
            maxWidth: "600px",
          }}
        >
          <Box className="flex justify-between items-center mb-4">
            <Typography variant="h6" className="text-white">
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
                  <Typography
                    variant="body2"
                    className="text-gray-300 flex flex-row items-center justify-center"
                    style={{ gap: "0.5rem" }}
                  >
                    <Typography variant="body2" className="text-gray-300">
                      Online:{" "}
                    </Typography>
                    {selectedUser.isOnline ? (
                      <CheckCircle
                        style={{ color: "#22c55e", fontSize: "1.2rem" }}
                      />
                    ) : (
                      <Close style={{ color: "#ef4444", fontSize: "1.2rem" }} />
                    )}
                  </Typography>
                </Box>
              </Box>
              <Box
                className="flex flex-col items-start"
                sx={{ marginLeft: "1rem", flex: 4 }}
              >
                <Typography variant="h5" className="text-white mb-2">
                  {selectedUser.name}
                </Typography>
                <Typography variant="body2" className="text-gray-300">
                  Total Playtime:{" "}
                  {formatTimeCount(selectedUser.totalLiveDuration)}
                </Typography>
                <Typography variant="body2" className="text-gray-300">
                  Last Online: {formatDateTime(selectedUser.spawnTime)}
                </Typography>
                {userRealmEvents && userRealmEvents.hasRealmEvents && (
                  <Box sx={{ width: "90%", }}>
                    <Typography
                      variant="h6"
                      className="text-white text-left"
                      sx={{ marginTop: "1.5rem" }}
                    >
                      Recent Events
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
