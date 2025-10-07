import { Injectable } from '@angular/core';
import axios from 'axios';
import { saveAs } from 'file-saver';

export interface Staff {
  staffId: string;
  fullName: string;
  birthday: string;
  gender: number;
}

@Injectable({ providedIn: 'root' })
export class StaffService {
  private baseUrl = 'https://localhost:5001/api/staffs'; // your ASP.NET backend URL

   async getAll(): Promise<Staff[]> {
    const res = await axios.get(this.baseUrl);
    return res.data;
  }

  async getById(id: string): Promise<Staff> {
    const res = await axios.get(`${this.baseUrl}/${id}`);
    return res.data;
  }

  async save(staff: Staff, isEdit: boolean) {
    if (isEdit)
    await axios.put(this.baseUrl, staff);
  else
    await axios.post(this.baseUrl, staff);
  }

  async delete(id: string) {
    await axios.delete(`${this.baseUrl}/${id}`);
  }

  async search(filters: {
    staffId?: string;
    gender?: number;
    startDate?: string;
    endDate?: string;
    export?: string;
  }): Promise<Staff[]> {
    const params = new URLSearchParams();

    if (filters.staffId) params.append('staffId', filters.staffId);
    if (filters.gender) params.append('gender', filters.gender.toString());
    if (filters.startDate) params.append('startDate', filters.startDate);
    if (filters.endDate) params.append('endDate', filters.endDate);
    if (filters.export) params.append('export', filters.export);

    const response = await axios.get<Staff[]>(`${this.baseUrl}?${params.toString()}`);
    
    return response.data;
  }

  async export(filters: {
    staffId?: string;
    gender?: number;
    startDate?: string;
    endDate?: string;
    export?: string;
  }): Promise<void> {
    const params = new URLSearchParams();

    const type = filters.export;

    if (filters.staffId) params.append('staffId', filters.staffId);
    if (filters.gender) params.append('gender', filters.gender.toString());
    if (filters.startDate) params.append('startDate', filters.startDate);
    if (filters.endDate) params.append('endDate', filters.endDate);
    if (filters.export) params.append('export', filters.export);

    // Use responseType 'blob' for file download
    const response = await axios.get(`${this.baseUrl}?${params.toString()}`, {
        responseType: 'blob',
    });

    const mimeType =
    type === 'EXCEL'
      ? 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
      : 'application/pdf';
    
    const blob = new Blob([response.data], { type: mimeType });
    const fileName = `Staffs_${new Date().toISOString()}.${type === 'EXCEL' ? 'xlsx' : 'pdf'}`;

    saveAs(blob, fileName);
  }
}