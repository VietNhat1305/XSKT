<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import {numeric, required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {pagingModel} from "@/models/pagingModel";
import Multiselect from "vue-multiselect";
import {userModel} from "@/models/userModel";
import {suKienModel} from "@/models/suKienModel";
import {youtubeModel} from "@/models/youtubeModel";
export default {
  page: {
    title: "Quản lý video",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader},
  data() {
    return {
      title: "Quản lý video",
      items: [
        {
          text: "Quản lý vidoe",
          href: '/quan-ly-video'
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      data: [],
      fields: [
        { key: 'STT',
          label: 'STT',
          class: 'td-stt',
          sortable: false,
          thClass: 'hidden-sortable'},
        {
          key: "name",
          label: "Tên",
          class: 'td-username',
          sortable: true,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        {
          key: "link",
          label: "Liên kết",
          class: 'td-ten',
          sortable: true,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        // {
        //   key: "unitRole",
        //   label: "Quyền",
        //   class: 'td-email',
        //   thStyle: "text-align:center",
        //   sortable: true,
        //   thClass: 'hidden-sortable'
        // },
        {
          key: 'process',
          label: 'Xử lý',
          class: 'text-center',
          sortable: false,
          thClass: 'hidden-sortable',
          thStyle: {width: '100px', minWidth: '60px'},
        }
      ],
      currentPage: 1,
      perPage: 10,
      pageOptions: [5, 10, 25, 50, 100],
      showModal: false,
      showDeleteModal: false,
      submitted: false,
      sortBy: "age",
      filter: null,
      sortDesc: false,
      filterOn: [],
      numberOfElement: 1,
      totalRows: 1,
      model: youtubeModel.baseJson(),
      listCoQuan: [],
      listRole: [],
      pagination: pagingModel.baseJson()
    };
  },
  computed: {
    //Validations
    // rules(){
    //   return{
    //     userName: {required},
    //     fullName: {required},
    //     unitRole: {required},
    //   }
    // }
  },
  validations: {
    model: {
      link: {required},
      name:{required}
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
        'manage-QUANLYVIDEO-655f1db60f560bd41a2decdc', 
        'create-QUANLYVIDEO-655f1db60f560bd41a2decdc',
        'update-QUANLYVIDEO-655f1db60f560bd41a2decdc',
        'delete-QUANLYVIDEO-655f1db60f560bd41a2decdc'
      ]
    }
  },
  methods: {
    normalizer(node){
      if(node.children == null || node.children == 'null'){
        delete node.children;
      }
    },
    async fnGetList() {
         this.$refs.tblList?.refresh()
    },
    // async getListRole(){
    //   await  this.$store.dispatch("unitRoleStore/getAll").then((res) =>{
    //     this.listRole = res.data || [];
    //   })
    // },
    async handleDelete() {
      if (this.model._id != 0 && this.model._id != null && this.model._id) {
        await this.$store.dispatch("linkYoutubeStore/delete",{_id :  this.model._id}).then((res) => {
          if (res.code===0) {
            this.fnGetList();
            this.showDeleteModal = false;
            this.model=youtubeModel.baseJson()
          }
          var a = {
            message: res.message,
            code: res.code
          };
       //   console.log("LOG A : " ,a)
          this.$store.dispatch("snackBarStore/addNotify", {
            message: res.message,
            code: res.code
          });
        });
      }
    },
    handleShowDeleteModal(id) {
      this.model._id = id;
      this.showDeleteModal = true;
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
          console.log("LOG USER UPDATE NE : " , this.model)
          // Update model
          await this.$store.dispatch("linkYoutubeStore/update", this.model).then((res) => {
            if (res.code === 0) {
              this.showModal = false;
              this.model=youtubeModel.baseJson()
              this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", {
              message: res.message,
              code: res.code,
            });
          });
        } else {
          // Create model
          console.log("LOG USER CREATE NE : " , this.model)
          await this.$store.dispatch("linkYoutubeStore/create", this.model).then((res) => {
            if (res.code === 0) {
              this.fnGetList();
              this.showModal = false;
              this.model={}
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
      await this.$store.dispatch("linkYoutubeStore/getById", {_id : id}).then((res) => {
        if (res.code===0) {
          console.log(res)
          this.model = youtubeModel.toJson(res.data);
          this.showModal = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", {
            message: res.message,
            code: res.code,
          });
        }
      });
    },

    myProvider (ctx) {
      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.filter,
        sortBy: 'thuTu',
      }
      this.loading = true
      try {
        let promise =  this.$store.dispatch("linkYoutubeStore/getPagingParams", params)
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
      if (status == false) this.model = youtubeModel.baseJson();
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
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row mb-2">
              <div class="col-sm-4">
                <div class="search-box me-2 mb-2 d-inline-block">
                  <div class="position-relative">
                    <input
                        v-model="filter"
                        type="text"
                        class="form-control"
                        placeholder="Tìm kiếm ..."
                    />
                    <i class="bx bx-search-alt search-icon"></i>
                  </div>
                </div>
              </div>
              <div class="col-sm-8">
                <div class="text-sm-end">
                  <b-button
                      v-if="$can('manage-QUANLYVIDEO-655f1db60f560bd41a2decdc') || $can('create-QUANLYVIDEO-655f1db60f560bd41a2decdc')"
                      type="button"
                      class="btn cs-btn-primary btn-rounded mb-2 me-2"
                      size="sm"
                      @click="showModal = true"
                  >
                    <i class="mdi mdi-plus me-1"></i> Thêm video
                  </b-button>
                  <b-modal
                      v-model="showModal"
                      title="Thông tin video"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                      size="lg"
                  >
                    <form @submit.prevent="handleSubmit"
                          ref="formContainer"
                    >
                      <div class="row">
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Tên video</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model._id"/>
                            <input
                                id="content"
                                v-model="model.name"
                                type="text"
                                class="form-control"
                                placeholder="Nhập tên video"
                                :class="{
                                'is-invalid':
                                  submitted && $v.model.name.$error,
                              }"
                            />
                            <div
                                v-if="submitted && !$v.model.name.required"
                                class="invalid-feedback"
                            >
                              Tên video không được trống.
                            </div>
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Liên kết video</label>
                            <span style="color: red">&nbsp;*</span>
                            <input type="hidden" v-model="model._id"/>
                            <input
                                id="content"
                                v-model="model.link"
                                type="link"
                                class="form-control"
                                placeholder="Nhập liên kết video"
                                :class="{
                                'is-invalid':
                                  submitted && $v.model.link.$error,
                              }"
                            />
                            <div
                                v-if="submitted && !$v.model.link.required"
                                class="invalid-feedback"
                            >
                              Liên kết video không được trống.
                            </div>
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-2">
                            <label class="text-left mb-0">Thứ tự</label>
                            <input
                                id="sort"
                                v-model="model.thuTu"
                                type="number"
                                class="form-control"
                                placeholder="Nhập thứ tự"
                            />
                          </div>
                        </div>

                      </div>
                      <div class="text-end pt-2 mt-3">
                        <b-button variant="light" @click="showModal = false" class="border-0">
                          Đóng
                        </b-button>
                        <b-button  type="submit" variant="success" class="ms-1 cs-btn-primary">Lưu
                        </b-button>
                      </div>
                    </form>
                  </b-modal>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-12">
                <div class="row mt-4">
                  <div class="col-sm-12 col-md-6">
                    <div
                        class="col-sm-12 d-flex justify-content-left align-items-center"
                    >
                      <div
                          id="tickets-table_length"
                          class="dataTables_length m-1"
                          style="
                        display: flex;
                        justify-content: left;
                        align-items: center;
                      "
                      >
                        <div class="me-1" >Hiển thị </div>
                        <b-form-select
                            class="form-select form-select-sm"
                            v-model="perPage"
                            size="sm"
                            :options="pageOptions"
                            style="width: 70px"
                        ></b-form-select
                        >&nbsp;
                        <div style="width: 100px"> dòng </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="table-responsive mb-0">
                  <b-table
                      class="datatables table-admin"
                      :items="myProvider"
                      :fields="fields"
                      striped
                      bordered
                      responsive="sm"
                      :per-page="perPage"
                      :current-page="currentPage"
                      :sort-by.sync="sortBy"
                      :sort-desc.sync="sortDesc"
                      :filter="filter"
                      :filter-included-fields="filterOn"
                      ref="tblList"
                      primary-key="id"
                  >
                    <template v-slot:cell(STT)="data">
                      {{ data.index + ((currentPage-1)*perPage) + 1  }}
                    </template>

                    <template v-slot:cell(process)="data">
                      <button
                          v-if="$can('manage-QUANLYVIDEO-655f1db60f560bd41a2decdc') || $can('update-QUANLYVIDEO-655f1db60f560bd41a2decdc')"
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          v-on:click="handleUpdate(data.item._id)">
                        <i class="fas fa-pencil-alt text-success me-1"></i>
                      </button>
                      <button
                          v-if="$can('manage-QUANLYVIDEO-655f1db60f560bd41a2decdc') || $can('delete-QUANLYVIDEO-655f1db60f560bd41a2decdc')"
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          v-on:click="handleShowDeleteModal(data.item._id)">
                        <i class="fas fa-trash-alt text-danger me-1"></i>
                      </button>
                    </template>
                  </b-table>
                </div>
                <div class="row">
                  <b-col>
                    <div>Hiển thị {{numberOfElement}} trên tổng số {{totalRows}} dòng</div>
                  </b-col>
                  <div class="col">
                    <div
                        class="dataTables_paginate paging_simple_numbers float-end">
                      <ul class="pagination pagination-rounded mb-0">
                        <!-- pagination -->
                        <b-pagination
                            v-model="currentPage"
                            :total-rows="totalRows"
                            :per-page="perPage"
                        ></b-pagination>
                      </ul>
                    </div>
                  </div>
                </div>

              </div>
            </div>
            <b-modal
                v-model="showDeleteModal"
                centered
                title="Xóa dữ liệu"
                title-class="font-18"
                no-close-on-backdrop
            >
              <p>
                Dữ liệu xóa sẽ không được phục hồi!
              </p>
              <template #modal-footer>
                <button v-b-modal.modal-close_visit
                        class="btn btn-outline-info m-1"
                        v-on:click="showDeleteModal = false">
                  Đóng
                </button>
                <button v-b-modal.modal-close_visit
                        class="btn btn-danger btn m-1"
                        v-on:click="handleDelete">
                  Xóa
                </button>
              </template>
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
  width: 20%
}
</style>
