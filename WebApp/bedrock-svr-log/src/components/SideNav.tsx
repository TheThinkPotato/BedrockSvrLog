import { Box, IconButton } from "@mui/material";
import {
  // AccountCircle,
  Map as MapIcon,
  Person as PersonIcon,
  Public as PublicIcon,
  Newspaper as NewspaperIcon,
} from "@mui/icons-material";

interface SideNavProps {
  handleShowSeedMap: () => void;
  handleUserStatsToggle: () => void;
  handleWorldStatsToggle: () => void;
  handleNewspaperToggle: () => void;
  userStatsOpen: boolean;
  worldStatsOpen: boolean;
  newspaperOpen: boolean;
}

const SideNav = ({
  handleShowSeedMap,
  handleUserStatsToggle,
  handleWorldStatsToggle,
  handleNewspaperToggle,
  userStatsOpen,
  worldStatsOpen,
  newspaperOpen,
}: SideNavProps) => {
  return (
    <Box className="w-16 bg-gray-800 border-l border-gray-700 flex flex-col items-center py-4 space-y-4 justify-between">
      <Box className="flex flex-col gap-2">
        <IconButton
          onClick={() => {
            handleShowSeedMap();
          }}
          className="text-blue-400 hover:text-blue-300"
          title="Map (Home)"
        >
          <MapIcon />
        </IconButton>

        <IconButton
          onClick={handleNewspaperToggle}
          className={`${
            newspaperOpen ? "text-yellow-400" : "text-gray-400"
          } hover:text-yellow-300`}
          title="Newspaper"
        >
          <NewspaperIcon />
        </IconButton>

        <IconButton
        
          onClick={handleUserStatsToggle}
          className={`${
            userStatsOpen ? "text-green-400" : "text-gray-400"
          } hover:text-green-300`}
          title="User Stats"
        >
          <PersonIcon />
        </IconButton>

        <IconButton
          onClick={handleWorldStatsToggle}
          className={`${
            worldStatsOpen ? "text-green-400" : "text-gray-400"
          } hover:text-green-300`}
          title="World Stats"
        >
          <PublicIcon />
        </IconButton>
      </Box>
      {/* <IconButton
          onClick={handleWorldStatsToggle}
          className={`${
            worldStatsOpen ? "text-green-400" : "text-gray-400"
          } hover:text-green-300`}
          title="World Stats"
        >
          <AccountCircle />
        </IconButton> */}
    </Box>
  );
};

export default SideNav;
