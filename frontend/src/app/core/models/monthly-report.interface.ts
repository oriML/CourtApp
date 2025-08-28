export interface MonthlyReport {
  id: string;
  month: number;
  year: number;
  totalRequests: number;
  resolvedRequests: number;
  unresolvedRequests: number;
  departmentBreakdown: { [key: string]: number }; // Added for "By Department"
}