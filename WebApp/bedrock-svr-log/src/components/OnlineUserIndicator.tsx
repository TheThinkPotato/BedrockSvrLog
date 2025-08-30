import { Avatar, Box, Tooltip } from "@mui/material";
import { useWorldData } from "../context/WorldContext";

const OnlineUserIndicator = () => {
  const { lastMessage } = useWorldData();

  return (
    <Box className="fixed z-50" style={{ top: "7.5rem", left: "7px" }}>
      <Box className="flex flex-col gap-2">
        <Box className="text-sm">
          {lastMessage?.OnlinePlayers.map((player) => (
            <Tooltip title={player.Name} placement="right">
              <Box
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
    </Box>
  );
};

export default OnlineUserIndicator;
