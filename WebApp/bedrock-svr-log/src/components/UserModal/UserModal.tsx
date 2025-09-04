import { Modal, Box, Typography, IconButton, Fade } from "@mui/material";
import { Close as CloseIcon } from "@mui/icons-material";
import { type Duration } from "../../Hooks/useGetDurations";
import { useGetUserRealmEvents } from "../../Hooks/useGetUserRealmEvetns";
import { useGetUserPlayerKills } from "../../Hooks/useGetUserPlayerKills";
import UserModalDetails from "./UserModalDetails";
import UserPlayerDetails from "./UserPlayerDetails";
import PlayerKillsTable from "./PlayerKillsTable";

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

  const { data: userPlayerKills, isLoading: userPlayerKillsLoading } = useGetUserPlayerKills(
    selectedUser?.xuid ?? 0,
    modalOpen
  );

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
                <UserPlayerDetails
                  selectedUser={selectedUser}
                  userPlayerKills={userPlayerKills ?? null}
                />
              </Box>
              <Box className="flex flex-col" sx={{ width: "80%" }}>
                <UserModalDetails
                  selectedUser={selectedUser}
                  userPlayerKills={userPlayerKills ?? null}
                  userRealmEvents={userRealmEvents ?? null}
                  userRealmEventsLoading={userRealmEventsLoading}
                />
                <PlayerKillsTable
                  userPlayerKills={userPlayerKills ?? null}
                  isLoading={userPlayerKillsLoading}
                />
              </Box>
            </Box>
          )}
        </Box>
      </Fade>
    </Modal>
  );
};

export default UserModal;
