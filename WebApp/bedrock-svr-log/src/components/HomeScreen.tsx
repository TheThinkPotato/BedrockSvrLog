import { Box } from '@mui/material'

const HomeScreen = () => {
  return (
    <Box className="w-full h-full p-4">
      <Box className="w-full h-full rounded-lg overflow-hidden border border-gray-700">
        <iframe
          src="map/index.html"
          title="Bedrock Server Map"
          className="w-full h-full border-0"
          sandbox="allow-same-origin allow-scripts allow-forms allow-popups allow-popups-to-escape-sandbox"
        />
      </Box>
    </Box>
  )
}

export default HomeScreen
