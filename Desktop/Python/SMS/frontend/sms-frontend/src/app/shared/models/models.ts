export interface Customer {
  customerId: number;
  code: string;
  name: string;
  address?: string;
  mobile?: string;
  email?: string;
  category?: string;
}

export interface Item {
  itemId: number;
  code: string;
  description: string;
  itemPhoto?: string;
  cost: number;
  salesPrice: number;
  unit?: string;
  brand?: string;
  supplier?: string;
}

export interface SalesLine {
  salesDetailId?: number;
  itemId: number;
  itemCode?: string;
  itemDescription?: string;
  unit?: string;
  qty: number;
  price: number;
  amount?: number;
}

export interface SalesInvoice {
  invoiceType: string;
  invoiceNumber: number;
  invoiceDate: string;
  customerId: number;
  customerName?: string;
  invoiceDescription?: string;
  totalAmount?: number;
  lines: SalesLine[];
}

export interface SalesReportRow {
  itemCode: string;
  itemDescription: string;
  itemPhoto?: string;
  unit?: string;
  customerName?: string;
  category?: string;
  brand?: string;
  qty: number;
  saleAmount: number;
  totalCost: number;
  grossProfit: number;
  invoiceDate: string;
}

export interface DashboardSummary {
  totalSales: number;
  totalGrossProfit: number;
  topSellingItem: string;
  salesTrend: { period: string; amount: number }[];
  salesByCategory: { category: string; amount: number }[];
}
