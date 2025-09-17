import { useState } from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";
import { Box} from "@mui/material";

import HomeScreen from "./components/Screens/HomeScreen";
import UserStatsDrawer from "./components/Drawers/UserStatsDrawer";
import WorldStatsDrawer from "./components/Drawers/WorldStatsDrawer";
import NewspaperModal from "./components/Modals/NewspaperModal/NewspaperModal";
import useGetWorld from "./Hooks/useGetWorld";
import SideNav from "./components/SideNav";

const queryClient = new QueryClient();

const theme = createTheme({
  palette: {
    mode: "dark",
    primary: {
      main: "#1976d2",
    },
    secondary: {
      main: "#dc004e",
    },
  },
});

function App() {
  const [userStatsOpen, setUserStatsOpen] = useState(false);
  const [worldStatsOpen, setWorldStatsOpen] = useState(false);
  const [showSeedMap, setShowSeedMap] = useState(false);
  const [newspaperOpen, setNewspaperOpen] = useState(false);

  const { data: world } = useGetWorld();

  const handleUserStatsToggle = () => {
    setUserStatsOpen(!userStatsOpen);
    if (worldStatsOpen) setWorldStatsOpen(false);
  };

  const handleShowSeedMap = () => {
    
    if (world?.seed) {
      setShowSeedMap(!showSeedMap);
    }
    
    if (userStatsOpen) setUserStatsOpen(false);
    if (worldStatsOpen) setWorldStatsOpen(false);
  };

  const handleWorldStatsToggle = () => {
    setWorldStatsOpen(!worldStatsOpen);
    if (userStatsOpen) setUserStatsOpen(false);
  };

  const handleNewspaperToggle = () => {
    setNewspaperOpen(!newspaperOpen);
    if (userStatsOpen) setUserStatsOpen(false);
    if (worldStatsOpen) setWorldStatsOpen(false);
  };

  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <Box className="flex h-screen bg-gray-900">
          {/* Main Content Area */}
          <Box className="flex-1 flex flex-col">
            <Box className="flex-1">
              <HomeScreen showSeedMap={showSeedMap} seed={world?.seed} />
            </Box>
          </Box>

          <SideNav
            handleShowSeedMap={handleShowSeedMap}
            handleUserStatsToggle={handleUserStatsToggle}
            handleWorldStatsToggle={handleWorldStatsToggle}
            handleNewspaperToggle={handleNewspaperToggle}
            userStatsOpen={userStatsOpen}
            worldStatsOpen={worldStatsOpen}
            newspaperOpen={newspaperOpen}
          />
          

          {/* User Stats Drawer */}
          <UserStatsDrawer
            open={userStatsOpen}
            onClose={() => setUserStatsOpen(false)}
          />

          {/* World Stats Drawer */}
          <WorldStatsDrawer
            open={worldStatsOpen}
            onClose={() => setWorldStatsOpen(false)}
          />

          {/* Newspaper Modal */}
          <NewspaperModal
            open={newspaperOpen}
            onClose={() => setNewspaperOpen(false)}
          />
        </Box>
      </ThemeProvider>
    </QueryClientProvider>
  );
}

export default App;
