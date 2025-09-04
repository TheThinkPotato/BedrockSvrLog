import { Box, Typography } from "@mui/material";
import { Column } from "primereact/column";
import { DataTable } from "primereact/datatable";
import { splitCamelCase } from "../../Helpers/textHelper";
import type { PlayerKillsList } from "../../Hooks/useGetUserPlayerKills";

interface KillTableProps {
  data: PlayerKillsList[];
  isLoading: boolean;
  showHeader: boolean;
}

const KillTable = ({ data, isLoading, showHeader = true }: KillTableProps) => {
  return (
    <>
      <Box>
        {showHeader && (
          <Typography
            variant="h6"
            sx={{ marginLeft: "0.5rem", marginBottom: "0.5rem" }}
          >
            Player Kills
          </Typography>
        )}
        <Box className="bg-gray-800 rounded-lg p-4">
          <DataTable
            emptyMessage="No kills recorded."
            value={data ?? []}
            className="text-white"
            scrollable
            scrollHeight="300px"
            stripedRows
            size="small"
            loading={isLoading}
          >
            <Column
              field="entityType"
              header="Creature"
              sortable 
              className="text-white"
              headerStyle={{ width: "220px" }}
              body={(item) => (
                <Typography>{splitCamelCase(item.entityType.replace("_", " "))}</Typography>
              )}
            />
            <Column
              field="killCount"
              header="Kills"
              sortable 
              className="text-white"
              body={(item) => <Typography>{item.killCount}</Typography>}
            />
          </DataTable>
        </Box>
      </Box>
    </>
  );
};

export default KillTable;
