import { Avatar, Box, Tooltip } from "@mui/material";
import { useWorldData } from "../context/WorldContext";

const OnlineUserIndicator = () => {
  const { lastMessage } = useWorldData();

  return (
    <Box className="fixed z-50" style={{ top: "7.5rem", left: "5px" }}>
      <Box className="flex flex-col gap-2">
        <Box className="text-sm">
          {lastMessage?.OnlinePlayers.map((player) => (
            <Tooltip title={player.Name} placement="right">
              <Box
                key={player.Name}
                style={{
                  backgroundColor: "rgba(255, 255, 255, 0.5)",
                  padding: "5px",
                  borderRadius: "100%",
                  marginBottom: "10px",
                  cursor: "pointer",
                }}
              >
                <Avatar
                  src={player.AvatarLink}
                  style={{ width: "28px", height: "28px" }}
                />
              </Box>
            </Tooltip>
          ))}
        </Box>
      </Box>
    </Box>
  );
};

export default OnlineUserIndicator;
