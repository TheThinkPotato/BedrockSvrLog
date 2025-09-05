import { Box, Fade, IconButton, Typography } from "@mui/material";

import { Modal } from "@mui/material";
import { useWorldData } from "../../../context/WorldContext";
import { minecraftTicksToTime } from "../../../Helpers/timeHelper";
import { Close as CloseIcon } from "@mui/icons-material";
import useGetUsers from "../../../Hooks/useGetUsers";

const WorldModal = ({
  modalOpen,
  handleModalClose,
}: {
  modalOpen: boolean;
  handleModalClose: () => void;
}) => {
  const { lastMessage: world } = useWorldData();

  const { data: users } = useGetUsers();
  const totalUsers = users?.length;

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
          <Box
            className="flex justify-between items-center"
            sx={{ marginBottom: "1rem" }}
          >
            <Typography
              variant="h5"
              fontWeight="bold"
              className="text-white"
              sx={{ marginLeft: "0.8rem" }}
            >
              World Details
            </Typography>
            <IconButton
              onClick={handleModalClose}
              className="text-gray-400 hover:text-white"
            >
              <CloseIcon />
            </IconButton>
          </Box>
          <Box
            className="flex flex-col items-start"
            sx={{ width: "100%", marginTop: "0.2rem" }}
          >
            <Box className="flex flex-col" sx={{ width: "100%" }}>
              <Box className="flex flex-row" sx={{ width: "100%" }}>
                <Typography
                  variant="body1"
                  className="text-gray-300"
                  sx={{ marginLeft: "0.8rem", width: "50%" }}
                >
                  <span style={{ fontWeight: "bold" }}>World Name: </span>
                  {world?.Name}
                </Typography>
                <Typography
                  variant="body1"
                  className="text-gray-300"
                  sx={{ marginLeft: "0.8rem" }}
                >
                  <span style={{ fontWeight: "bold" }}>World Seed: </span>
                  {world?.Seed}
                </Typography>
              </Box>

              <Box
                className="flex flex-row"
                sx={{ width: "100%", marginTop: "0.1rem" }}
              >
                <Typography
                  variant="body1"
                  className="text-gray-300"
                  sx={{ marginLeft: "0.8rem", width: "50%" }}
                >
                  <span style={{ fontWeight: "bold" }}>Game Day: </span>
                  {world?.CurrentDay}
                </Typography>
                <Typography
                  variant="body1"
                  className="text-gray-300"
                  sx={{ marginLeft: "0.8rem" }}
                >
                  <span style={{ fontWeight: "bold" }}>Game Time: </span>
                  {minecraftTicksToTime(world?.CurrentTime ?? NaN)}
                </Typography>
              </Box>
              <Box
                className="flex flex-row"
                sx={{ width: "100%", marginTop: "1.5rem" }}
              >
                <Typography
                  variant="body1"
                  className="text-gray-300"
                  sx={{ marginLeft: "0.8rem", width: "50%" }}
                >
                  <span style={{ fontWeight: "bold" }}>Total Players: </span>
                  {totalUsers}
                </Typography>
                <Typography
                  variant="body1"
                  className="text-gray-300"
                  sx={{ marginLeft: "0.8rem" }}
                >
                  <span style={{ fontWeight: "bold" }}>Online Players: </span>
                  {world?.OnlinePlayers.length}
                </Typography>
              </Box>
            </Box>
          </Box>
        </Box>
      </Fade>
    </Modal>
  );
};

export default WorldModal;
