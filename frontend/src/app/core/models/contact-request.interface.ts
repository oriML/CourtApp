export interface ContactRequest {
  id: string;
  name: string;
  email: string;
  phone?: string;
  departments?: string[];
  description: string;
  status: 'pending' | 'resolved' | 'rejected';
  createdAt: string;
}