import { useState } from 'react'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { ThemeProvider, createTheme } from '@mui/material/styles'
import CssBaseline from '@mui/material/CssBaseline'
import { Box,IconButton } from '@mui/material'
import { Map as MapIcon, Person as PersonIcon, Public as PublicIcon } from '@mui/icons-material'
import HomeScreen from './components/HomeScreen'
import UserStatsDrawer from './components/UserStatsDrawer'
import WorldStatsDrawer from './components/WorldStatsDrawer'

const queryClient = new QueryClient()

const theme = createTheme({
  palette: {
    mode: 'dark',
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#dc004e',
    },
  },
})

function App() {
  const [userStatsOpen, setUserStatsOpen] = useState(false)
  const [worldStatsOpen, setWorldStatsOpen] = useState(false)

  const handleUserStatsToggle = () => {
    setUserStatsOpen(!userStatsOpen)
    if (worldStatsOpen) setWorldStatsOpen(false)
  }

  const handleWorldStatsToggle = () => {
    setWorldStatsOpen(!worldStatsOpen)
    if (userStatsOpen) setUserStatsOpen(false)
  }

  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <Box className="flex h-screen bg-gray-900">
          {/* Main Content Area */}
          <Box className="flex-1 flex flex-col">
            <Box className="flex-1">
              <HomeScreen />
            </Box>
          </Box>

          {/* Right Side Menu */}
          <Box className="w-16 bg-gray-800 border-l border-gray-700 flex flex-col items-center py-4 space-y-4">
            <IconButton
              onClick={() => {
                setUserStatsOpen(false)
                setWorldStatsOpen(false)
              }}
              className="text-blue-400 hover:text-blue-300"
              title="Map (Home)"
            >
              <MapIcon />
            </IconButton>
            
            <IconButton
              onClick={handleUserStatsToggle}
              className={`${userStatsOpen ? 'text-green-400' : 'text-gray-400'} hover:text-green-300`}
              title="User Stats"
            >
              <PersonIcon />
            </IconButton>
            
            <IconButton
              onClick={handleWorldStatsToggle}
              className={`${worldStatsOpen ? 'text-green-400' : 'text-gray-400'} hover:text-green-300`}
              title="World Stats"
            >
              <PublicIcon />
            </IconButton>
          </Box>

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
        </Box>
      </ThemeProvider>
    </QueryClientProvider>
  )
}

export default App
