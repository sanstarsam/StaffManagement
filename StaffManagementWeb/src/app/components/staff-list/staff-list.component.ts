import { Component, OnInit } from '@angular/core';
import { StaffService, Staff } from 'src/app/services/staff.service';


@Component({
  selector: 'app-staff-list',
  templateUrl: './staff-list.component.html',
  styleUrls: ['./staff-list.component.css']
})
export class StaffListComponent implements OnInit {
  staffs: Staff[] = [];
  showModal = false;
  isEdit = false;
  staffForm: Staff = { staffId: '', fullName: '', birthday: '', gender: 1 };
  filters = {
    staffId: '',
    gender: 0,
    startDate: '',
    endDate: '',
    export: ''
  };

  constructor(private staffService: StaffService) {}

  async ngOnInit() {
    await this.load();
  }

  async load() {
    this.staffs = await this.staffService.getAll();
  }

  openAdd() {
    this.isEdit = false;
    this.staffForm = { staffId: '', fullName: '', birthday: '', gender: 1 };
    this.showModal = true;
  }

  openEdit(staff: Staff) {
    this.isEdit = true;
    this.staffForm = { ...staff };
    this.showModal = true;
  }

  close() {
    this.showModal = false;
  }

  async save() {
    if (!this.staffForm.fullName.trim()) return alert('Full name required!');
    await this.staffService.save(this.staffForm, this.isEdit);
    this.showModal = false;
    await this.load();
  }

  async delete(id: string) {
    if (confirm('Delete this staff?')) {
      await this.staffService.delete(id);
      await this.load();
    }
  }

  async onSearch() {
    this.filters.export = '';
    this.staffs = await this.staffService.search(this.filters);
  }

  async onExport(type: 'EXCEL' | 'PDF') {
    this.filters.export = type;
    await this.staffService.export(this.filters);
  }
}