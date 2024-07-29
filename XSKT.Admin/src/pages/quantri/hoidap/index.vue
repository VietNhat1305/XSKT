<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import { numeric, required } from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import { pagingModel } from "@/models/pagingModel";
import Multiselect from "vue-multiselect";
import { contentModel } from "@/models/contentModel";
import DatePicker from "vue2-datepicker";
import Treeselect from "@riophae/vue-treeselect";
export default {
  page: {
    title: "Hỏi đáp",
    meta: [{ name: "description", content: appConfig.description }],
  },
  components: { Layout, PageHeader },
  data() {
    return {
      title: "Hỏi đáp",
      items: [
        {
          text: "Hỏi đáp",
          href: '/danh-sach-hoi-dap'
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      data: [],
      fields: [
        {
          key: 'STT',
          label: 'STT',
          class: 'td-stt',
          sortable: false,
          thClass: 'hidden-sortable'
        },
        {
          key: "moTa",
          label: "Câu hỏi",
          class: 'td-username',
          sortable: false,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        {
          key: "createdAtShow",
          label: "Ngày",
          class: 'td-username',
          sortable: false,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        {
          key: "noiDung",
          label: "Câu trả lời",
          class: 'td-username',
          sortable: false,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        {
          key: "email",
          label: "Email",
          class: 'td-username',
          sortable: false,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        {
          key: "isPublic",
          label: "Trạng thái",
          class: 'td-trangthai text-center',
          sortable: false,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        {
          key: "traloi",
          label: "Tình trạng",
          class: 'td-trangthai text-center',
          sortable: false,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        {
          key: "files",
          label: "Files",
          class: 'td-trangthai text-center',
          sortable: false,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: 'td-xuly',
          sortable: false,
          thClass: 'hidden-sortable'
        }
      ],
      currentPage: 1,
      perPage: 10,
      pageOptions: [5, 10, 25, 50, 100],
      showModal: false,
      showDeleteModal: false,
      showCreateHoiDap: false,
      submitted: false,
      sortBy: "age",
      filter: null,
      sortDesc: false,
      filterOn: [],
      numberOfElement: 1,
      totalRows: 1,
      model: contentModel.baseJson(),
      listCoQuan: [],
      listRole: [],
      pagination: pagingModel.baseJson(),
      itemFilter: {
        content: null,
        ngayBatDau: null,
        ngayKetThuc: null,
        menuId: null
      },
      treeView: [],
      url: `${process.env.VUE_APP_API_URL}filesminio/view`,
    };
  },
  computed: {
    // //Validations
    // rules() {
    //   return {
    //     userName: { required },
    //     fullName: { required },
    //     unitRole: { required },
    //   }
    // }
  },
  validations: {
    model: {
      // name: { required },
      // hoVaTen:{required},
      // noiDung:{required},
      moTa:{required},
    },
  },
  created() {
    const user = {
      id: 1,
      name: 'Victory Osayi',
      is_editor: true,
      is_admin: false,
      // you can have role based permission list or access control list possibly from database
      permissions: [
        'manage-HOIDAP-6567e935c9549992c800fe4c', 
        'create-HOIDAP-6567e935c9549992c800fe4c',
        'update-HOIDAP-6567e935c9549992c800fe4c',
        'delete-HOIDAP-6567e935c9549992c800fe4c'
      ]
    }
  },
  methods: {
    normalizer(node) {
      if (node.children == null || node.children == 'null') {
        delete node.children;
      }
    },
    async fnGetList() {
      this.$refs.tblList?.refresh()
    },
    handleShowDeleteModal(id) {
      this.model._id = id;
      this.showDeleteModal = true;
    },
    handleShowCreateHoiDap(id) {
      this.model._id = id;
      this.showCreateHoiDap = true;
    },
    async handleSubmit(e) {
      e.preventDefault();
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        if (
          this.model._id != 0 &&
          this.model._id != null &&
          this.model._id
        ) {
          console.log("LOG USER UPDATE NE : ", this.model)
          // Update model
          await this.$store.dispatch("contentStore/updateHoiDap", this.model).then((res) => {
            if (res.code === 0) {
              this.showModal = false;
              this.model = contentModel.baseJson()
              this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", {
              message: res.message,
              code: res.code,
            });
          });
        } else {
          // Create model
          console.log("LOG USER CREATE NE : ", this.model)
          this.model.menu=[];
          this.model.menu.push({id : "6544b8331f222fcfc365c913" , name : "Hỏi đáp"});
          await this.$store.dispatch("contentStore/createHoiDap", this.model).then((res) => {
            if (res.code === 0) {
              this.fnGetList();
              this.showCreateHoiDap = false;
              this.model = {}
            }
            this.$store.dispatch("snackBarStore/addNotify", {
              message: res.message,
              code: res.code,
            });
          });
        }
        loader.hide();
      }
      this.submitted = false;
    },
    async handleUpdate(id) {
      await this.$store.dispatch("contentStore/getById", { "_id": id }).then((res) => {
        if (res.code === 0) {
          console.log(res)
          this.model = contentModel.toJson(res.data);
          this.showModal = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", {
            message: res.message,
            code: res.code,
          });
        }
      });
    },
    async handleDelete() {
      if (this.model._id != 0 && this.model._id != null && this.model._id) {
        await this.$store.dispatch("contentStore/delete", { '_id': this.model._id }).then((res) => {
          if (res.code === 0) {
            this.fnGetList();
            this.showDeleteModal = false;
          }
          var a = {
            message: res.message,
            code: res.code
          };
          // console.log("LOG A : " ,a)
          this.$store.dispatch("snackBarStore/addNotify", {
            message: res.message,
            code: res.code
          });
        });
      }
    },
    addCoQuanToModel(node, instanceId) {
      if (node.id) {
        this.itemFilter.menuId = node.id;
      }
    },
    myProvider(ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.itemFilter.content,
        menuId: this.itemFilter.menuId,
        startDate: this.itemFilter.ngayBatDau
          ? new Date(this.itemFilter.ngayBatDau)
          : null,
        endDate: this.itemFilter.ngayKetThuc
          ? new Date(this.itemFilter.ngayKetThuc)
          : null,
        sortDesc: ctx.sortDesc,
      }
      this.loading = true
      try {
        let promise = this.$store.dispatch("contentStore/getPagingParamsHoiDap", params)
        return promise.then(resp => {
          let items = resp.data.data
          this.totalRows = resp.data.totalRows
          this.numberOfElement = resp.data.data.length
          this.loading = false
          return items || []
        })
      } finally {
        this.loading = false
      }
    },
    handleSearch() {
      // console.log("LOG HANDLE SEARCH LICH SU GIAO DICH " , this.itemFilter)
      this.$refs.tblList.refresh();
    },
    handleClear() {
      this.itemFilter = {
        content: null,
        ngayBatDau: null,
        ngayKetThuc: null,
        menuId: null
      };
    },
  },
  mounted() {

  },
  watch: {
    model: {
      deep: true,
      handler(val) {
        // addCoQuanToModel()
        // this.saveValueToLocalStorage()
      }
    },
    showModal(status) {
      if (status == false) this.model = contentModel.baseJson();
    },
    // model() {
    //   return this.model;
    // },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    }

  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items" />
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row">
              <div class="col-12">
                <div class="row mt-4 mb-2">
                  <div class="col-md-9 d-flex justify-content-left align-items-center">
                    <div id="tickets-table_length" class="dataTables_length m-1" style="
                      display: flex;
                      justify-content: left;
                      align-items: center;
                    ">
                      <div class="me-1">Hiển thị </div>
                      <b-form-select class="form-select form-select-sm" v-model="perPage" size="sm"
                        :options="pageOptions" style="width: 70px"></b-form-select>&nbsp;
                      <div style="width: 100px"> dòng </div>
                    </div>
                  </div>
                  <div class="col-md-3 d-flex" style="justify-content: end; align-items: center;">
                      <div class="text-sm-end">
                        <b-button 
                          v-if="$can('manage-HOIDAP-6567e935c9549992c800fe4c') || $can('create-HOIDAP-6567e935c9549992c800fe4c')"
                          type="button" 
                          class="btn cs-btn-primary btn-rounded mb-2 me-2" 
                          size="sm"
                          @click="showCreateHoiDap = true"
                        >
                          <i class="mdi mdi-plus me-1"></i> Thêm hỏi đáp
                        </b-button>
                      </div>
                    </div>
                </div>
                <b-modal v-model="showModal" title="Thông tin câu hỏi" title-class="text-black font-18" body-class="p-3"
                  hide-footer centered no-close-on-backdrop size="lg">
                  <form @submit.prevent="handleSubmit" ref="formContainer">
                    <div class="row">
                      <div class="col-12">
                        <div class="mb-3">
                          <label class="text-left">Nội dung câu hỏi </label>
                          <input id="content" v-model="model.moTa" type="text" class="form-control"
                            placeholder="Nhập nội dung" disabled />
                        </div>
                      </div>
                      <div class="col-12">
                        <div class="form-check form-switch mb-2">
                          <label class="text-left mb-2">Xuất bản</label>
                          <input v-model="model.isPublic" class="form-check-input" type="checkbox"
                            id="flexSwitchCheckChecked" checked="" />
                        </div>
                      </div>
                      <div class="col-12">
                        <div class="mb-3">
                          <label class="text-left">Địa chỉ</label>
                          <input id="content" v-model="model.diaChi" type="text" class="form-control"
                            placeholder="Nhập địa chỉ" disabled />
                        </div>
                      </div>
                      <div class="col-12">
                        <div class="mb-3">
                          <label class="text-left">Trả lời </label>
                          <textarea id="content" v-model="model.noiDung" type="text" class="form-control"
                            placeholder="Nhập nội dung">
                            </textarea>
                        </div>
                      </div>

                    </div>
                    <div class="text-end pt-2 mt-3">
                      <b-button variant="light" @click="showModal = false" class="border-0">
                        Đóng
                      </b-button>
                      <b-button type="submit" variant="success" class="ms-1 cs-btn-primary">Lưu
                      </b-button>
                    </div>
                  </form>
                </b-modal>
                <div class="table-responsive mb-0">
                  <b-table class="datatables table-admin" :items="myProvider" :fields="fields" striped bordered
                    responsive="sm" :per-page="perPage" :current-page="currentPage" :sort-by.sync="sortBy"
                    :sort-desc.sync="sortDesc" :filter="filter" :filter-included-fields="filterOn" ref="tblList"
                    primary-key="id">
                    <template v-slot:cell(STT)="data">
                      {{ data.index + ((currentPage - 1) * perPage) + 1 }}
                    </template>
                    <!-- <template v-slot:cell(menu)="data">
                     <span style="margin-left: 5px">
                       {{data.item.menu.name}}
                     </span>
                    </template> -->
                    <template v-slot:cell(menu)="data">
                      <div v-for="(item, index) in data.item.menu" :key="index">
                        <span class="badge bg-warning mb-1"
                          style="padding: 5px; color: #1e2125; text-transform: uppercase; font-weight: bold;">{{ item.name
                          }}</span>
                      </div>
                    </template>
                    <template v-slot:cell(isPublic)="data">
                      <span v-if="data.item.isPublic" class="badge bg-success" style="padding: 5px;">Xuất bản</span>
                      <span v-else class="badge" style="padding: 5px; background-color: red;">Không xuất bản</span>
                    </template>
                    <template v-slot:cell(traloi)="data">
                      <span v-if="data.item.noiDung != null && data.item.noiDung != ''" class="badge bg-success" style="padding: 5px;">Đã trả
                        lời</span>
                      <span v-else class="badge" style="padding: 5px; background-color: red;">Chưa trả lời</span>
                    </template>
                    <template v-slot:cell(files)="data">
                      <div v-for ="(item,index) in data.item.files" :key="index">
                        <a
                            :href="`${url}/${item.fileId}`"
                            class="text-dark fw-medium d-flex justify-content-center align-items-center">
                          <div class="">
                            <!-- <i
                                :class="`${getColorWithExtFile(item.ext)} me-2 ${getIconWithExtFile(item.ext)}`"
                            ></i>
                            <span style="font-weight: bold">{{ item.fileName }}</span> -->
                            <i class="mdi mdi-cloud-download" style="font-size: 24px; color: #B50027;"></i>
                          </div>
                        </a>
                      </div>
                    </template>
                    <template v-slot:cell(process)="data">
                      <!-- <router-link
                          :to="{
                                      path: `/danh-sach/chi-tiet/${data.item._id}`,
                                    }">
                        <button
                            type="button"
                            size="sm"
                            class="btn btn-outline btn-sm"
                            >
                          <i class="fas fa-pencil-alt text-success me-1"></i>
                        </button>
                      </router-link> -->
                      <button 
                        v-if="$can('manage-HOIDAP-6567e935c9549992c800fe4c') || $can('update-HOIDAP-6567e935c9549992c800fe4c')"
                        type="button" 
                        size="sm" 
                        class="btn btn-outline btn-sm"
                        v-on:click="handleUpdate(data.item._id)"
                      >
                        <i class="fas fa-pencil-alt text-success me-1"></i>
                      </button>
                      <button 
                        v-if="$can('manage-HOIDAP-6567e935c9549992c800fe4c') || $can('delete-HOIDAP-6567e935c9549992c800fe4c')"
                        type="button" 
                        size="sm" 
                        class="btn btn-outline btn-sm"
                        v-on:click="handleShowDeleteModal(data.item._id)"
                      >
                        <i class="fas fa-trash-alt text-danger me-1"></i>
                      </button>
                    </template>
                  </b-table>
                </div>
                <div class="row">
                  <b-col>
                    <div>Hiển thị {{ numberOfElement }} trên tổng số {{ totalRows }} dòng</div>
                  </b-col>
                  <div class="col">
                    <div class="dataTables_paginate paging_simple_numbers float-end">
                      <ul class="pagination pagination-rounded mb-0">
                        <!-- pagination -->
                        <b-pagination v-model="currentPage" :total-rows="totalRows" :per-page="perPage"></b-pagination>
                      </ul>
                    </div>
                  </div>
                </div>

              </div>
            </div>
            <b-modal v-model="showDeleteModal" centered title="Xóa dữ liệu" title-class="font-18" no-close-on-backdrop>
              <p>
                Dữ liệu xóa sẽ không được phục hồi!
              </p>
              <template #modal-footer>
                <button v-b-modal.modal-close_visit class="btn btn-outline-info m-1" v-on:click="showDeleteModal = false">
                  Đóng
                </button>
                <button v-b-modal.modal-close_visit class="btn btn-danger btn m-1" v-on:click="handleDelete">
                  Xóa
                </button>
              </template>
            </b-modal>
            <!-- <b-modal v-model="showCreateHoiDap" centered title="Tạo câu hỏi" title-class="font-18" no-close-on-backdrop> -->
              <b-modal
                      v-model="showCreateHoiDap"
                      title="Tạo câu hỏi"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="lg"
                  >
              <form @submit.prevent="handleSubmit" ref="formContainer">
                <div class="row">
                  <div class="col-4">
                    <div class="mb-3">
                      <label class="text-left">Nhập tiêu đề</label>
                      <!-- <span style="color: red">&nbsp;*</span> -->
                      <input id="name" v-model.trim="model.name" type="text" class="form-control"
                        placeholder="Nhập tiêu đề" />
                    </div>
                  </div>

                  <div class="col-4">
                    <div class="mb-3">
                      <label class="text-left">Họ và Tên</label>
                      <input id="hoVaTen" v-model="model.hoVaTen" type="text" class="form-control"
                        placeholder="Nhập họ và tên"  />
                    </div>
                  </div>
                  <div class="col-4">
                    <div class="mb-3">
                      <label class="text-left">Số điện thoại</label>
                      <input id="soDienThoai" v-model="model.soDienThoai" type="text" class="form-control"
                        placeholder="Nhập số điện thoại" />
                    </div>
                  </div>
                  <div class="col-12">
                    <div class="mb-3">
                      <label class="text-left">Địa chỉ</label>
                      <input id="soDienThoai" v-model="model.diaChi" type="text" class="form-control"
                        placeholder="Nhập địa chỉ" />
                    </div>
                  </div>

                  <div class="col-12">
                    <div class="mb-3">
                      <label class="text-left">Nhập câu hỏi</label>
                      <span style="color: red">&nbsp;*</span>
                      <input id="name" v-model.trim="model.moTa" type="text" class="form-control"
                        placeholder="Nhập câu hỏi" :class="{
                          'is-invalid':
                            submitted && $v.model.moTa.$error,
                        }" />
                      <div v-if="submitted && !$v.model.moTa.required" class="invalid-feedback">
                        Câu hỏi không được trống.
                      </div>
                    </div>
                  </div>

                  <div class="col-12">
                    <div class="mb-3">
                      <label class="text-left">Nội dung câu trả lời</label>
                      <span style="color: red">&nbsp;*</span>
                      <input type="hidden" v-model="model.noiDung" />
                      <textarea id="noiDung" v-model="model.noiDung" type="text" class="form-control"
                        placeholder="Nhập nội dung" :class="{
                          'is-invalid':
                            submitted && $v.model.noiDung.$error,
                        }" />
                      <div v-if="submitted && !$v.model.noiDung.required" class="invalid-feedback">
                        Nội dung không được trống.
                      </div>
                    </div>
                  </div>
                  <div class="col-12">
                        <div class="form-check form-switch mb-2">
                          <label class="text-left mb-2">Xuất bản</label>
                          <input v-model="model.isPublic" class="form-check-input" type="checkbox"
                            id="flexSwitchCheckChecked" checked="" />
                        </div>
                      </div>
                </div>
                <div class="text-end pt-2 mt-3">
                  <b-button type="button" variant="warning" class="ms-1" v-on:click="showCreateHoiDap = false"> Đóng
                    lại
                  </b-button>

                  <b-button type="submit" variant="success" class="ms-1 cs-btn-primary px-3">Lưu câu hỏi
                  </b-button>
                </div>

              </form>
            </b-modal>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>
<style>
.td-stt {
  text-align: center;
}

.td-xuly {
  text-align: center;
  width: 10% !important;
}

.td-chuyenmuc {
  width: 20%;
}

.td-trangthai {
  width: 100px;
}

.vue-treeselect__control {
  height: 39px !important;
}

.mx-table .mx-table-date .table thead th,
thead,
th {
  background-color: rgb(255, 255, 255) !important;
  color: #0e0e0e !important;
  border: none;
}
</style>
