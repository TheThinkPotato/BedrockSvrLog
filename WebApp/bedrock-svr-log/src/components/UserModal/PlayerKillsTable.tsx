import Box from "@mui/material/Box";

import type { UserPlayerKills } from "../../Hooks/useGetUserPlayerKills";
import { Typography } from "@mui/material";
import KillTable from "../KillTable";

interface PlayerKillsTableProps {
  userPlayerKills: UserPlayerKills | null;
  isLoading: boolean;
}

const PlayerKillsTable = ({
  userPlayerKills,
  isLoading,
}: PlayerKillsTableProps) => {
  return (
    <Box
      className="flex flex-col items-start"
      sx={{ marginLeft: "2rem", flex: 4 }}
    >
      <Box sx={{ width: "90%" }}>
        <Typography
          variant="h6"
          className="text-white text-left"
          sx={{ marginTop: "1.5rem" }}
        >
          Player Kills
        </Typography>
        <KillTable
          data={userPlayerKills?.playerKillsList ?? []}
          isLoading={isLoading}
          showHeader={false}
        />
      </Box>
    </Box>
  );
};

export default PlayerKillsTable;
