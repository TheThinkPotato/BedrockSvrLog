import { Avatar, Box, Tooltip } from "@mui/material";
import { useWorldData, type OnlinePlayers } from "../context/WorldContext";
import { useState } from "react";
import UserModal from "./Modals/UserModal/UserModal";

interface OnlineUserIndicatorProps {
  showSeedMap: boolean;
}

const OnlineUserIndicator = ({ showSeedMap }: OnlineUserIndicatorProps) => {
  const { lastMessage } = useWorldData();
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedUser, setSelectedUser] = useState<OnlinePlayers | null>(null);

  const handleUserClick = (player: OnlinePlayers) => {
    setSelectedUser(player);
    setModalOpen(true);
  };

  return (
    <Box
      className="fixed z-50"
      style={{
        ...(!showSeedMap
          ? { left: "7px", top: "7.5rem" }
          : { left: "1rem", top: "10rem" }),
      }}
    >
      <Box className="flex flex-col gap-2">
        <Box className="text-sm">
          {lastMessage?.OnlinePlayers.reduce((distinct, player) => {
            if (!distinct.find((p) => p.Name === player.Name)) {
              distinct.push(player);
            }
            return distinct;
          }, [] as OnlinePlayers[]).map((player) => (
            <Tooltip title={player.Name} key={player.Name} placement="right">
              <Box
                onClick={() => handleUserClick(player)}
                key={player.Name}
                sx={{
                  backgroundColor: "rgba(255, 255, 255, 1)",
                  padding: "3px",
                  borderRadius: "100%",
                  marginBottom: "10px",
                  cursor: "pointer",
                  opacity: 0.8,
                  "&:hover": {
                    opacity: 1,
                  },
                }}
              >
                <Avatar
                  src={player.AvatarLink}
                  sx={{ width: "28px", height: "28px" }}
                />
              </Box>
            </Tooltip>
          ))}
        </Box>
      </Box>
      <UserModal
        selectedUserXuid={selectedUser?.Xuid ?? NaN}
        handleModalClose={() => setModalOpen(false)}
        modalOpen={modalOpen}
      />
    </Box>
  );
};

export default OnlineUserIndicator;
